
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {

        [Required]
        [MaxLength(20, ErrorMessage = "Symbol cannot be over 20 characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(40, ErrorMessage = "Symbol cannot be over 40 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1,10000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.01,100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Industry cannot be over 20 characters")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1,5000000000)]
        public long MarketCup { get; set; }
    }
}
