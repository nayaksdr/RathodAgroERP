using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class SplitwiseRepository : ISplitwiseRepository
    {
        private readonly ApplicationDbContext _context;

        public SplitwiseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Members
        public async Task<List<Member>> GetMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }
        public async Task AddExpenseAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();  // डेटा DB मध्ये खरंच सेव्ह होण्यासाठी हे गरजेचं आहे
        }
        public async Task<List<Expense>> GetExpensesAsync()
        {
            return await _context.Expenses
                .Include(e => e.ExpenseType)  // Load ExpenseType
                .Include(e => e.PaidBy)       // Load Member
                .ToListAsync();
        }

        public async Task AddMemberAsync(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
        }

        // ✅ Expenses


        //public async Task AddExpenseAsync(Expense expense)
        //{
        //    _context.Expenses.Add(expense);
        //    await _context.SaveChangesAsync();
        //}

        // ✅ Balances      
        public async Task<Dictionary<int, decimal>> GetBalancesAsync()
        {
            // Get all members and expenses from the database
            var members = await _context.Members.ToListAsync();
            var expenses = await _context.Expenses.ToListAsync();

            // Initialize balances with 0 for each member
            var balances = members.ToDictionary(m => m.Id, m => 0m);

            foreach (var expense in expenses)
            {
                List<int> shareList;

                if (expense.SharedByIds == null || expense.SharedByIds.Count == 0)
                {
                    // If SharedByIds is null/empty, assume all other members share the expense
                    shareList = members
                        .Where(m => m.Id != expense.PaidById)
                        .Select(m => m.Id)
                        .ToList();
                }
                else
                {
                    shareList = expense.SharedByIds
                        .Where(id => id != expense.PaidById) // ignore PaidById if present
                        .ToList();
                }

                // Split amount equally among all members in shareList
                if (shareList.Count > 0)
                {
                    var share = expense.Amount / shareList.Count;

                    foreach (var memberId in shareList)
                    {
                        balances[memberId] -= share;          // member owes money
                        balances[expense.PaidById] += share;  // paid member gets credit
                    }
                }
                else
                {
                    // If no one else is sharing, full amount goes to PaidBy
                    balances[expense.PaidById] += expense.Amount;
                }
            }

            return balances;
        }

        // ✅ Settlements
        public async Task<List<(int from, int to, decimal amount)>> GetSettlementsAsync()
        {
            var balances = await GetBalancesAsync();
            var transactions = new List<(int from, int to, decimal amount)>();

            // Threshold to ignore tiny rounding errors
            const decimal epsilon = 0.01m;

            // debtors (< -epsilon), creditors (> epsilon)
            var debtors = balances
                .Where(b => b.Value < -epsilon)
                .OrderBy(b => b.Value)
                .ToList();

            var creditors = balances
                .Where(b => b.Value > epsilon)
                .OrderByDescending(b => b.Value)
                .ToList();

            int i = 0, j = 0;

            while (i < debtors.Count && j < creditors.Count)
            {
                var debtor = debtors[i];
                var creditor = creditors[j];

                // किती पैसे देणं आहे हे ठरव
                decimal amount = Math.Min(-debtor.Value, creditor.Value);

                if (amount > epsilon)
                    transactions.Add((debtor.Key, creditor.Key, amount));

                // update balances
                balances[debtor.Key] += amount;
                balances[creditor.Key] -= amount;

                // move to next debtor/creditor if balance is close to zero
                if (balances[debtor.Key] >= -epsilon) i++;
                if (balances[creditor.Key] <= epsilon) j++;
            }

            return transactions;
        }



        // Get single expense by Id
        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses
                .Include(e => e.ExpenseType)  // Include related ExpenseType
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Update an existing expense
        public async Task UpdateExpenseAsync(Expense expense)
        {
            var existing = await _context.Expenses.FindAsync(expense.Id);
            if (existing == null) throw new KeyNotFoundException("Expense not found");

            // Update fields
            //existing.Description = expense.Description;
            existing.Amount = expense.Amount;
            existing.PaidById = expense.PaidById;
            existing.ExpenseTypeId = expense.ExpenseTypeId;
            existing.SharedByIds = expense.SharedByIds;
            // 🔹 New fields to include
            existing.PaymentMode = expense.PaymentMode;  // ✅ Cash / UPI / Bank
            existing.ProofImage = expense.ProofImage;    // ✅ image path from controller
            existing.Date = expense.Date;

            _context.Expenses.Update(existing);
            await _context.SaveChangesAsync();
        }

        // Delete an expense
        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) throw new KeyNotFoundException("Expense not found");

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SplitwisePayment>> GetPaymentsAsync() =>
            await _context.SplitwisePayments
                .Include(p => p.FromMember)
                .Include(p => p.ToMember)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

        public async Task AddPaymentAsync(SplitwisePayment payment)
        {
            _context.SplitwisePayments.Add(payment);

            // Update balances
            var from = await _context.Members.FindAsync(payment.FromMemberId);
            var to = await _context.Members.FindAsync(payment.ToMemberId);

            if (from != null && to != null)
            {
                from.Balance -= payment.Amount;
                to.Balance += payment.Amount;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateMemberBalanceAsync(int memberId, decimal newBalance)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member != null)
            {
                member.Balance = newBalance;
                await _context.SaveChangesAsync();
            }
        }
    }
}
