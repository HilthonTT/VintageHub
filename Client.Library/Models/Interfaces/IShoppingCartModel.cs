namespace Client.Library.Models.Interfaces;

public interface IShoppingCartModel
{
    List<CartItemModel> CartItems { get; set; }
}