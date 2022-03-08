namespace Basket.API.Entities;

public class ShoppingCart
{
    public ShoppingCart(string username)
    {
        Username = username;
    }

    public ShoppingCart()
    {
    }

    public string Username { get; set; } = string.Empty;
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    public decimal TotalPrice 
    {
        get
        {
            return Items.Sum(x => x.Price * x.Quantity);
        }
    }
}
