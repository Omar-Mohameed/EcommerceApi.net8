using System.Text.Json.Serialization;

namespace Test.Core.Entities.Order
{
    public class OrderItem : BaseEntity<int>
    {
        public OrderItem(int productItemId, string mainImage, string productName, decimal price, int quntity)
        {
            ProductItemId = productItemId;
            MainImage = mainImage;
            ProductName = productName;
            Price = price;
            Quntity = quntity;
        }
        public OrderItem(){}

        public int ProductItemId { get; set; }
        public string MainImage { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quntity { get; set; }

        [JsonIgnore]
        public int OrderId { get; set; }
        [JsonIgnore]
        public Orders Order { get; set; }
    }
}