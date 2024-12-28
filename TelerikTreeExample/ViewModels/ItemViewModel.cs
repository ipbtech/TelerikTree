using System.ComponentModel.DataAnnotations;

namespace TelerikTreeExample.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        
        [Required]
        [Display(Name = "ItemName")]
        public string Name { get; set; }

        [DataType("Double")]
        [Display(Name = "ItemValue")]
        public double? Value { get; set; }

        [Display(Name = "ItemValue")]
        public double? AggregateValue { get; set; }
    }
}
