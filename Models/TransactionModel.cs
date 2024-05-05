using System;

namespace CashierManagement.Models
{
    public class TransactionModel
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
    }
}