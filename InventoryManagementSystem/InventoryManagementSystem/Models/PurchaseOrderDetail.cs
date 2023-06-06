﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models
{
    public class PurchaseOrderDetail
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [ForeignKey("PurchaseOrderHeader")]
        public int PurchaseOrderHeaderId { get; set; }
        public virtual PurchaseOrderHeader PurchaseOrderHeader { get; private set; }


        [Required]
        [ForeignKey("Product")]
        [MaxLength(6)]
        public string ProductCode { get; set; }
        public virtual Product Product { get; private set; }


        [Column(TypeName = "smallmoney")]
        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity should be greater than 0 and less than 1000")]
        public decimal Quantity { get; set; }


        [Range(1, 10000000, ErrorMessage = "FOB should be greater than 0")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "smallmoney")]
        [Required]
        public decimal Fob { get; set; } //Free on Board= Suppliers price in Foreign Currency


        [Range(1, 10000000, ErrorMessage = "Price should be greater than 0")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "smallmoney")]
        [Required]
        public decimal PriceInBaseCurrency { get; set; }


        [MaxLength(75)]
        [NotMapped]
        public string Description { get; set; } = "";

        [MaxLength(25)]
        [NotMapped]
        public string UnitName { get; set; } = "Pcs";

        [NotMapped]
        public bool IsDeleted { get; set; } = false;
    }
}
