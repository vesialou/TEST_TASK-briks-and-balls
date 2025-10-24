namespace BricksAndBalls.Core.Interfaces
{
    public interface IStorageService
    {
        void Save<T>(string key, T data);
        T Load<T>(string key, T defaultValue = default);
        bool HasKey(string key);
        void DeleteKey(string key);
        void DeleteAll();
    }
}
