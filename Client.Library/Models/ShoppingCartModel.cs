using Client.Library.Models.Interfaces;

namespace Client.Library.Models;
public class ShoppingCartModel : IShoppingCartModel
{
    public List<CartItemModel> CartItems { get; set; } = new();
}
