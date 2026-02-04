import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { SplitwiseService } from '../../services/splitwise';
import Chart from 'chart.js/auto';
import { Subscription, forkJoin } from 'rxjs'; // Added forkJoin
import { SplitwiseSummaryModel } from '../../core/models/splitwise-summary.model';
import * as XLSX from 'xlsx';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import Swal from 'sweetalert2';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-splitwise',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MatIconModule],
  templateUrl: './splitwise.html',
  styleUrls: ['./splitwise.css']
})
export class SplitwiseComponent implements OnInit, OnDestroy {
  expenseForm!: FormGroup;
  memberForm!: FormGroup;
  isMemberLoading: boolean = false;
  isEditMode: boolean = false;
  currentEditId: number | null = null;
  isLoading: boolean = false;

  members: any[] = [];
  expenses: any[] = [];
  settlements: any[] = [];
  expenseTypes: any[] = [];
  
  searchText: string = '';
  selectedFile: File | null = null;
  imagePreview: string | null = null;
  
  pieChart: Chart | null = null;
  barChart: Chart | null = null;
  private subscriptions: Subscription = new Subscription();

  constructor(private notify: NotificationService,private fb: FormBuilder, private api: SplitwiseService) {}

  ngOnInit(): void {
    this.initializeForms();
    this.loadExpenseTypes();
    this.listenToPaymentModeChanges();
    this.refreshData(); // This now handles everything centrally
  }

  ngOnDestroy(): void {
    this.pieChart?.destroy();
    this.barChart?.destroy();
    this.subscriptions.unsubscribe();
  }

  initializeForms(): void {
    // Note: Used lowercase keys to match typical JSON responses
    this.expenseForm = this.fb.group({
      paidById: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(1)]],
      expenseTypeId: [1, Validators.required],
      paymentMode: ['Cash', Validators.required],
      date: [new Date().toISOString().substring(0, 10), Validators.required]
    });

    this.memberForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  loadExpenseTypes() {
    this.api.getExpenseTypes().subscribe({
      next: (data) => this.expenseTypes = data,
      error: (err) => console.error('Failed to load types', err)
    });
  }

  listenToPaymentModeChanges(): void {
    const sub = this.expenseForm.get('paymentMode')?.valueChanges.subscribe(mode => {
      if (mode === 'Cash') {
        this.selectedFile = null;
        this.imagePreview = null;
      }
    });
    if (sub) this.subscriptions.add(sub);
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = () => this.imagePreview = reader.result as string;
      reader.readAsDataURL(file);
    }
  }
// Add this inside the SplitwiseComponent class
resetMemberForm(): void {
  if (this.memberForm) {
    this.memberForm.reset();
  }
  this.isMemberLoading = false;
  // If you are using this inside a modal, you can also add logic here to close it
}
  refreshData(): void {
    this.isLoading = true;
    // Using forkJoin to ensure we have members BEFORE calculating settlements
    const sub = this.api.getDashboardSummary().subscribe({
      next: (data: SplitwiseSummaryModel) => {
        this.members = data.members || [];
        this.expenses = data.expenses || [];
        
        // Calculate settlements locally based on the refreshed expense list
        this.calculateSettlements(this.expenses);
        
        setTimeout(() => this.initCharts(), 150);
        this.isLoading = false;
      },
      error: (err) => {
        console.error("API Error:", err);
        this.isLoading = false;
      }
    });
    this.subscriptions.add(sub);
  }

  // ================= SETTLEMENT LOGIC (FIXED) =================

  calculateSettlements(expenses: any[]) {
    if (!expenses || expenses.length === 0 || this.members.length === 0) {
      this.settlements = [];
      return;
    }

    const balances: { [key: number]: number } = {};
    // 1. Source names from the members list
    const namesMap: { [key: number]: string } = {};
    this.members.forEach(m => {
      const id = Number(m.id || m.Id);
      balances[id] = 0;
      namesMap[id] = m.name;
    });

    // 2. Add total paid from expenses
    expenses.forEach(exp => {
      const pId = Number(exp.paidById || exp.PaidById);
      if (balances.hasOwnProperty(pId)) {
        balances[pId] += Number(exp.amount || exp.Amount);
      }
    });

    // 3. Calculate equal share
    const totalSpent = expenses.reduce((sum, exp) => sum + Number(exp.amount || exp.Amount), 0);
    const sharePerPerson = totalSpent / this.members.length;

    // 4. Final Balance = Paid - Share
    const memberBalances = this.members.map(m => {
      const id = Number(m.id || m.Id);
      return {
        id: id,
        name: namesMap[id] || 'Unknown',
        netBalance: balances[id] - sharePerPerson
      };
    });

    // 5. Separate into strict Payer/Receiver groups
    let debtors = memberBalances.filter(b => b.netBalance < -0.01).sort((a, b) => a.netBalance - b.netBalance);
    let creditors = memberBalances.filter(b => b.netBalance > 0.01).sort((a, b) => b.netBalance - a.netBalance);

    this.settlements = [];
    let d = 0, c = 0;

    while (d < debtors.length && c < creditors.length) {
      const debtor = debtors[d];
      const creditor = creditors[c];
      const amount = Math.min(Math.abs(debtor.netBalance), creditor.netBalance);
      
      if (amount > 0.01) {
        this.settlements.push({
          fromId: debtor.id,
          fromName: debtor.name,
          toId: creditor.id,
          toName: creditor.name,
          amount: Math.round(amount)
        });
      }

      debtor.netBalance += amount;
      creditor.netBalance -= amount;
      if (Math.abs(debtor.netBalance) < 0.1) d++;
      if (Math.abs(creditor.netBalance) < 0.1) c++;
    }
  }

  // ================= ACTIONS =================

  saveExpense(): void {
    if (this.expenseForm.invalid) {
    Swal.fire({
      icon: 'error',
      title: 'Oops...',
      text: 'Please fill all required fields!',
      confirmButtonColor: '#0d6efd' // Bootstrap Primary color
    });
    return;
  }
    if (this.expenseForm.invalid) return;
const payload = this.expenseForm.value;
    if (this.expenseForm.get('paymentMode')?.value !== 'Cash' && !this.selectedFile && !this.isEditMode) {
      alert("Receipt is required for Online/UPI payments.");
      return;

    }

    const formData = new FormData();
    Object.entries(this.expenseForm.value).forEach(([key, value]) => {
      if (value !== null) formData.append(key, value as string);
    });

    if (this.selectedFile) formData.append('ProofImage', this.selectedFile);

    const action$ = (this.isEditMode && this.currentEditId)
      ? this.api.updateExpense(this.currentEditId, formData)
      : this.api.addExpense(formData);
action$.subscribe({
    next: (res) => {
      this.isLoading = false;
      
      // SUCCESS ALERT
      Swal.fire({
        title: this.isEditMode ? 'Updated!' : 'Added!',
        text: `Expense has been ${this.isEditMode ? 'updated' : 'added'} successfully.`,
        icon: 'success',
        timer: 2000,
        showConfirmButton: false
      });

     this.resetExpenseForm();
        this.refreshData(); // Refresh your list
      // Close the bootstrap modal programmatically if needed
    },
    error: (err) => {
      this.isLoading = false;
      this.notify.showError('Error saving expense data: ' + err.message);
         
    }
  });
}

updateExpense(expense: any) {
  this.isEditMode = true;
  this.currentEditId = expense.id || expense.Id;

  // Patching values into the form
  this.expenseForm.patchValue({
    paidById: expense.paidById || expense.PaidById,
    amount: expense.amount || expense.Amount,
    expenseTypeId: expense.expenseTypeId || expense.ExpenseTypeId,
    paymentMode: expense.paymentMode || expense.PaymentMode,
    // Formats date to YYYY-MM-DD for the HTML input
    date: expense.date ? expense.date.substring(0, 10) : new Date().toISOString().substring(0, 10)
  });

  // Note: window.scrollTo is usually not needed if using a Modal
}

  async deleteExpense(id: number) {
  // CONFIRMATION DIALOG
  const result = await this.notify.confirm(
    'Delete Expense?', 
    'This action cannot be undone.'
  );

  if (result.isConfirmed) {
    this.notify.showLoading('Deleting...'); // 2. Show progress

    this.api.deleteExpense(id).subscribe({
      next: () => {
        this.notify.close();
        this.notify.showSuccess('Expense deleted.'); // 3. Success
        this.refreshData(); // Use refreshData instead of loadExpenses
      },
      error: () => {
        this.notify.close();
        this.notify.showError('Could not delete this item.');
      }
    });
  }
}

// Add this inside the SplitwiseComponent class
async settlePayment(fromId: number, toId: number, amount: number){
  // Use the global confirmation
  const result = await this.notify.confirm(
    'Confirm Settlement', 
    `Are you sure you want to settle ₹${amount}?`
  );

  if (result.isConfirmed) {
    const payload = { fromMemberId: fromId, toMemberId: toId, amount: amount };

    this.api.pay(payload).subscribe({
      next: () => {
        this.notify.showSuccess('Payment settled successfully!');
        this.refreshData();
      },
      error: () => this.notify.showError('Could not process settlement.')
    });
  }
}
  saveMember(): void {
    if (this.memberForm.invalid){
       this.notify.showError('Please fill in all required fields.');
       return;
    }
    this.isMemberLoading = true; // Start spinner
     this.notify.showLoading(this.isEditMode ? 'Updating your member...' : 'Adding new member...');
    this.api.addMember(this.memberForm.value).subscribe(() => {
      this.notify.close(); // Close loading indicator
      this.notify.showSuccess(this.isEditMode ? 'Member Updated!' : 'Member Added!');
      this.resetMemberForm(); // Use the function here!
      this.memberForm.reset();
      this.refreshData();
    });
  }

  public resetExpenseForm(): void {
    this.expenseForm.reset({ 
      paymentMode: 'Cash', 
      expenseTypeId: 1,
      date: new Date().toISOString().substring(0, 10) 
    });
    this.isEditMode = false;
    this.currentEditId = null;
    this.selectedFile = null;
    this.imagePreview = null;
  }

  // ================= CHARTS & HELPERS =================

  initCharts(): void {
    const ctxPie = document.getElementById('expensePieChart') as HTMLCanvasElement;
    const ctxBar = document.getElementById('balanceBarChart') as HTMLCanvasElement;
    if (!ctxPie || !ctxBar) return;

    this.pieChart?.destroy();
    this.barChart?.destroy();

    const labels = this.members.map(m => m.name);
    const expenseSums = this.members.map(m => 
      this.expenses.filter(e => (e.paidById || e.PaidById) == (m.id || m.Id))
      .reduce((s, c) => s + Number(c.amount || c.Amount || 0), 0)
    );

    this.pieChart = new Chart(ctxPie, {
      type: 'doughnut',
      data: {
        labels,
        datasets: [{ data: expenseSums, backgroundColor: ['#6366f1', '#8b5cf6', '#ec4899', '#f59e0b', '#10b981'] }]
      },
      options: { responsive: true, maintainAspectRatio: false }
    });

    const balances = this.members.map(m => {
       const id = Number(m.id || m.Id);
       const paid = this.expenses.filter(e => Number(e.paidById || e.PaidById) === id).reduce((s, c) => s + Number(c.amount || c.Amount), 0);
       return paid - (this.calculateGrandTotal() / this.members.length);
    });

    this.barChart = new Chart(ctxBar, {
      type: 'bar',
      data: {
        labels,
        datasets: [{ label: 'Balance (₹)', data: balances, backgroundColor: balances.map(v => v >= 0 ? '#10b981' : '#ef4444') }]
      },
      options: { responsive: true, maintainAspectRatio: false }
    });
  }

  calculateGrandTotal(): number {
    return this.expenses.reduce((sum, curr) => sum + Number(curr.amount || curr.Amount || 0), 0);
  }

  get filteredExpenses() {
    if (!this.searchText) return this.expenses;
    return this.expenses.filter(e =>
      (e?.paidByName || e?.paidBy?.name || '').toLowerCase().includes(this.searchText.toLowerCase())
    );
  }
  // ================= EXPORT ACTIONS =================

  exportExcel(): void {
  if (this.expenses.length === 0) {
    this.notify.showError('No data available to export.');
    return;
  }

  // 1. Prepare the data for Excel
  const dataToExport = this.expenses.map(e => ({
    'Date': e.date ? e.date.substring(0, 10) : 'N/A',
    'Paid By': e.paidByName || e.paidBy?.name || 'Unknown',
    'Category': e.expenseTypeName || 'General',
    'Payment Mode': e.paymentMode || 'Cash',
    'Amount (₹)': Number(e.amount || 0)
  }));

  // 2. Create a new Worksheet
  const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataToExport);

  // 3. Create a new Workbook
  const workbook: XLSX.WorkBook = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(workbook, worksheet, 'Expenses');

  // 4. Set column widths (Optional but recommended)
  const wscols = [
    { wch: 15 }, // Date
    { wch: 20 }, // Paid By
    { wch: 15 }, // Category
    { wch: 15 }, // Mode
    { wch: 12 }  // Amount
  ];
  worksheet['!cols'] = wscols;

  // 5. Save and Download
  const fileName = `Expenses_Report_${new Date().toISOString().substring(0, 10)}.xlsx`;
  XLSX.writeFile(workbook, fileName);
}

  exportPdf(): void {
  const doc = new jsPDF();
  doc.text('Expense Settlement Report', 14, 20);
  
  const data = this.expenses.map(e => [
    e.date, 
    e.paidByName || 'Member', 
    e.paymentMode, 
    `₹${e.amount}`
  ]);

  autoTable(doc, {
    head: [['Date', 'Paid By', 'Mode', 'Amount']],
    body: data,
    startY: 30
  });

  doc.save('expenses.pdf');
  this.notify.showSuccess('PDF Exported Successfully');
}
}