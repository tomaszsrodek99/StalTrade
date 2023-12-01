﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StalTradeAPI.Models
{
    public class PriceHistory
    {
        public int PriceHistoryID { get; set; }
        [Required]
        public int ProductID { get; set; }
        public Product Product { get; set; }
        [Required]
        [Column(TypeName = "decimal(5, 2)"), Display(Name = "Cena")]
        public decimal Price { get; set; }
        [Required]
        public bool IsPurchase { get; set; } // Prawda jeżeli zakup
        [Required, Display(Name = "Data")]
        public DateTime EventDate { get; set; }        
    }
}
