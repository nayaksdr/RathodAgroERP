using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IPaymentRepository
    {
        Task<List<WeeklyPayment>> GetWeeklyPaymentsAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<bool> IsPaymentAlreadyDoneAsync(DateTime fromDate, DateTime toDate);
        Task SaveWeeklyPaymentsAsync(List<WeeklyPayment> payments);

        //IEnumerable<WeeklyPayment> GetPayments();       
        IEnumerable<WeeklyPayment> GetPaymentsAll(int laborId, DateTime fromDate, DateTime toDate);
        IEnumerable<WeeklyPayment> GetWeeklyPayments();
        
        IEnumerable<Payment> GetAll();
        Payment GetById(int id);        
        void Add(Payment payment);
        void AddPayment(WeeklyPayment payment);
        void Update(Payment payment);
        void Delete(int id);
        void Save();
    }
}
