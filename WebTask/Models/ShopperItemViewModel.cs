namespace WebTask.Models
{
    public class ShopperItemViewModel
    {
        public int SelectedShopperId { get; set; }
        public int SelectedItemId { get; set; }

        public List<Shopper> Shoppers { get; set; } = new List<Shopper>();
        public List<ShoppingItem> ShoppingItems { get; set; } = new List<ShoppingItem>();

    }
}
