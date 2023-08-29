namespace Shared.Library.LocalStorage.Interfaces;
public interface IShoppingCartStorage
{
    Task<ShoppingCartModel> GetShoppingCartAsync();
    Task SaveShoppingCartAsync(ShoppingCartModel shoppingCart);
}