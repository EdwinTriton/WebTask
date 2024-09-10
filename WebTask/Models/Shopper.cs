using System.ComponentModel.DataAnnotations;

namespace WebTask.Models
{
    public class Shopper
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public List<ShoppingItem> ShoppingItems { get; set; } = new List<ShoppingItem>();


    }
}
