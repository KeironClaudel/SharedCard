namespace SharedCard.Api.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public DateTime BillDate { get; set; }
        public string Commerce { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public int PersonId { get; set; }
        public int CycleId { get; set; }
        public string? Notes { get; set; }
        public DateTime LoggedAt { get; set; }
        public Person Person { get; set; } = null!;
        public BillingCycle BillingCycle { get; set; } = null!;
    }
}
