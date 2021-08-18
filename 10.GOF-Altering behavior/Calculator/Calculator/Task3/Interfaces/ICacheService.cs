namespace Calculator.Task3.Interfaces
{
    public interface ICacheService<T>
    {
        T Get(string key);
        void Set(string key, T value);
    }
}
