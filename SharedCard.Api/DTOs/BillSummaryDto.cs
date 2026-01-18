namespace SharedCard.Api.DTOs
{
    public class BillSummaryDto
    {
        public int PersonId { get; set; }
        public string Name { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = null!;
    }
}
