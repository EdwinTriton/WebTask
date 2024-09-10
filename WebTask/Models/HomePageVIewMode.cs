using System.Collections.Generic;
using WebTask.Models;

namespace WebTask.Models
{
    public class HomePageViewModel
    {
        public List<Shopper> Shoppers { get; set; } = new List<Shopper>();
        public List<ShoppingItem>? ShoppingItems { get; set; }
    }
}
