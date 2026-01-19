namespace SharedCard.Api.DTOs
{
    public class BillResponseDto
    {
        public int BillId { get; set; }
        public DateTime BillDate { get; set; }
        public string Commerce { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public int PersonId { get; set; }
        public int CycleId { get; set; }
    }
}
