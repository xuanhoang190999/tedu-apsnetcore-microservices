using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public abstract class CreateOrUpdateProductDto // Gom các thuộc tính chung nhất ở Create và Update, giúp việc xem sự khác biệt giữa update và create rõ ràng hơn
    {
        [Required]
        [MaxLength(250, ErrorMessage = "Maximum length for Product Name is 250 characters.")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "Maximum length for Product Summary is 255 characters.")]
        public string Summary { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
