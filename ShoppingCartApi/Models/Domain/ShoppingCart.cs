namespace ShoppingCartApi.Models.Domain
{
    public class ShoppingCart
    {
        public Guid ShoppingCartID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }

        public Guid OrderID { set; get; }
    }
}
