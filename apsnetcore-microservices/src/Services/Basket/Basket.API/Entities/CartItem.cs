using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities
{
    public class CartItem
    {
        [Required]
        [Range(1, double.PositiveInfinity, ErrorMessage = "The filed {0} must be >= {1}")] // Giá trị phải lớn hơn 1, double.PositiveInfinity số dương vô cực
        public int Quantity { get; set; }

        [Required]
        [Range(0.1, double.PositiveInfinity, ErrorMessage = "The filed {0} must be >= {1}")] // Giá trị phải lớn hơn 1, double.PositiveInfinity số dương vô cực
        public decimal ItemPrice { get; set; }

        [Required]
        public string ItemNo { get; set; }

        [Required]
        public string ItemName { get; set; }
    }
}
