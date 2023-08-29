namespace Shared.Library.LocalStorage.Interfaces;

public interface ILocalStorage
{
    Task<T> GetAsync<T>(string key);
    Task RemoveAsync(string key);
    Task SetAsync<T>(string key, T value, TimeSpan cacheValidity);
}