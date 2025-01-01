using System.ComponentModel.DataAnnotations;

namespace TelerikTreeExample.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        [DataType("Integer")]
        public int? ParentId { get; set; }
        
        [Required]
        [Display(Name = "ItemName")]
        public string Name { get; set; }

        [DataType("Decimal")]
        public double? Value { get; set; }

        [Display(Name = "ItemValue")]
        [DataType("Decimal")]
        public double? AggregateValue { get; set; }
    }
}
