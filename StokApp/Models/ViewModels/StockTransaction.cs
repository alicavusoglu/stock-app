using System;
using System.ComponentModel.DataAnnotations;

namespace StokApp.Models.ViewModels
{
    public class StockTransactionListVM
    {
        public int Id { get; set; }

        [Display(Name ="Ürün")]
        public string ProductText { get; set; }

        [Display(Name = "Miktar")]
        public string Amount { get; set; }

        [Display(Name = "Tarih")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public string Date { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }

    public class StockTransactionEditVM
    {
        public int Id { get; set; }

        [Display(Name = "Ürün")]
        public int ProductRef { get; set; }

        [Required]
        [Display(Name = "Miktar")]
        public int? Amount { get; set; }

        public StockTransactionType TransactionType { get; set; }

        [Display(Name = "Tarih")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }

    public enum StockTransactionType
    {
        Input = 1,
        Output = 2
    }
}