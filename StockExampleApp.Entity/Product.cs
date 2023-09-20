using System.ComponentModel.DataAnnotations;

namespace StockExampleApp.Entity
{
    public class Product
    {
        [Required(ErrorMessage = "Urun kodu alani bos kalamaz")]
        public int productId { get; set; }

        [Required(ErrorMessage = "Urun ismi alani bos kalamaz")]
        public string productName { get; set; }

        [Required(ErrorMessage = "Urun miktar alani bos kalamaz")]
        public int productQuantity { get; set; }

        [Required(ErrorMessage = "Urun fiyat alani bos kalamaz")]
        public double productPrice { get; set; }

    }
}
