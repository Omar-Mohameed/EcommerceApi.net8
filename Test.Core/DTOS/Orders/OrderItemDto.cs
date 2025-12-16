namespace Ecom.Core.DTOS.Orders
{
    public record OrderItemDto
    {
        public int ProductItemId { get; set; }
        public string MainImage { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quntity { get; set; }
    }
}