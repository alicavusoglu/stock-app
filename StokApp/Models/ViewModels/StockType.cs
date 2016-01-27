using System.ComponentModel.DataAnnotations;

namespace StokApp.Models.ViewModels
{
    public class StockTypeVM
    {
        public  int Id { get; set; }

        [Display(Name="Birim Adı")]
        public string TypeName { get; set; }

        [Display(Name = "Birimdeki Parça Adedi")]
        public int PiecesInType { get; set; }
    }
}
