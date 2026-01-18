namespace SharedCard.Api.Models
{
    public class BillingCycle
    {
        public int CycleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CutOff { get; set; }
        public DateTime DueDate { get; set; }
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}
