using Newtonsoft.Json;
using WebTask.Models;

namespace WebTask.Data
{
    public static class Serialize
    {
        public const string FilePathShoppers = "Data/shoppers.json";
        public const string FilePathShoppingItems = "Data/shopping_items.json";
        public static void SerializeShoppers(List<Shopper> shoppers)
        {
            using (StreamWriter writer = new StreamWriter(FilePathShoppers))
            {
                var serializedData = JsonConvert.SerializeObject(shoppers);
                writer.Write(serializedData);
            }
        }

        public static List<Shopper>? DeserializeShoppers()
        {
            try
            {
                using (StreamReader reader = new StreamReader(FilePathShoppers))
                {
                    var serializedData = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<Shopper>>(serializedData);
                }
            }

            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error deserializing shopping items: {ex.Message}");
                return null;
            }
        }
        public static void SerializeShoppingItems(List<ShoppingItem> shoppingItems)
        {
            using (StreamWriter writer = new StreamWriter(FilePathShoppingItems))
            {
                var serializedData = JsonConvert.SerializeObject(shoppingItems);
                writer.Write(serializedData);
            }
        }
        
        public static List<ShoppingItem>? DeserializeShoppingItems()
        {
            try
            {
                using (StreamReader reader = new StreamReader(FilePathShoppingItems))
                {
                    var serializedData = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<ShoppingItem>>(serializedData);
                }
            }

            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error deserializing shopping items: {ex.Message}");
                return null;
            }
        }
    }
}
