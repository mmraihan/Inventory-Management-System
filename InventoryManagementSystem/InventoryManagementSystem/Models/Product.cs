using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models
{
    public class Product
    {
        //[Remote("IsProductCodeValid", "Product", AdditionalFields = "Name", ErrorMessage = "Product Code Exists Already")]
        [Key]
        [StringLength(6)]
        public string Code { get; set; }

        //[Remote("IsProductNameValid", "Product", AdditionalFields = "Code", ErrorMessage = "Product Name Exists Already")]
        [Required]
        [StringLength(75)]
        public String Name { get; set; }

        [Required]
        [StringLength(255)]
        public String Description { get; set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal Cost { get; set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey("Units")]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }
        public virtual Unit Units { get; set; } // Navigation Prperty

    }
}
