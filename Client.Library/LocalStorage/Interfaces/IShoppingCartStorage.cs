using Client.Library.Models;

namespace Client.Library.LocalStorage.Interfaces;
public interface IShoppingCartStorage
{
    Task<ShoppingCartModel> GetShoppingCartAsync();
    Task SaveShoppingCartAsync(ShoppingCartModel shoppingCart);
}