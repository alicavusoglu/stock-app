using System;
using System.ComponentModel.DataAnnotations;

namespace StokApp.Models.ViewModels
{
    public class ProductListVM
    {
        public int Id;

        [Display(Name = "Ürün Adı")]
        public string Name;

        public DateTime CreatedDate;

        [Display(Name = "Stok Birimi")]
        public string StockTypeText;

        [Display(Name = "Stok Durumu")]
        public int Amount { get; set; }
    }

    //public class ProductEditVMv
    //{
    //    public int Id;

    //    [Required]
    //    [Display(Name = "Ürün Adı")]
    //    public string Name;

    //    public DateTime CreatedDate;

    //    [Required]
    //    [Display(Name = "Stok Birimi")]
    //    public int StockTypeRef;
    //}
}
