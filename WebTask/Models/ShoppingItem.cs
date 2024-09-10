using System.ComponentModel.DataAnnotations;

namespace WebTask.Models
{
    public class ShoppingItem
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CurrentAvailableStock { get; set; } = 3;

    }
}
