using System.ComponentModel.DataAnnotations;

namespace SharedCardAPI.DTOs
{
    public class UpdateBillDto
    {
        [Required]
        public DateTime BillDate { get; set; }

        [Required]
        [MaxLength(150)]
        public string Commerce { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression("CRC|USD")]
        public string Currency { get; set; } = "CRC";

        [Required]
        public int PersonId { get; set; }

        [MaxLength(250)]
        public string? Notes { get; set; }
    }
}

