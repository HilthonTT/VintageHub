namespace Client.Library.LocalStorage;
public class ShoppingCartStorage : IShoppingCartStorage
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromDays(14);
    private const string CacheName = nameof(ShoppingCartStorage);
    private readonly ILocalStorage _localStorage;

    public ShoppingCartStorage(ILocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<ShoppingCartModel> GetShoppingCartAsync()
    {
        var output = await _localStorage.GetAsync<ShoppingCartModel>(CacheName);
        output ??= new();

        return output;
    }

    public async Task SaveShoppingCartAsync(ShoppingCartModel shoppingCart)
    {
        await _localStorage.SetAsync(CacheName, shoppingCart, CacheTimeSpan);
    }
}
