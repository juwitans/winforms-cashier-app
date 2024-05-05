namespace CashierManagement.Models
{
    public class TransactionDetail
    {
        public int TransactionId { get; set; }
        public string DrugId { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}