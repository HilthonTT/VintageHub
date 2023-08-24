namespace Client.Library.LocalStorage;

public class LocalStorage : ILocalStorage
{
    private readonly ILocalStorageService _localStorage;

    public LocalStorage(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan cacheDuration)
    {
        var cachedData = new CachedData<T>
        {
            Data = value,
            Expiration = DateTimeOffset.UtcNow.Add(cacheDuration)
        };

        await _localStorage.SetItemAsync(key, cachedData);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var cachedData = await _localStorage.GetItemAsync<CachedData<T>>(key);

        if (cachedData is not null && cachedData.Expiration > DateTimeOffset.UtcNow)
        {
            return cachedData.Data;
        }

        return default;
    }

    public async Task RemoveAsync(string key)
    {
        await _localStorage.RemoveItemAsync(key);
    }
}

