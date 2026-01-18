using System.ComponentModel.DataAnnotations;

namespace SharedCard.Api.DTOs
{
    public class CreateBillDto
    {
        [Required]
        public DateTime BillDate { get; set; }

        [Required, MaxLength(150)]
        public string Commerce { get; set; } = null!;

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required, RegularExpression("CRC|USD")]
        public string Currency { get; set; } = null!;

        [Required]
        public int PersonId { get; set; }

        [MaxLength(250)]
        public string? Notes { get; set; }

    }
}
