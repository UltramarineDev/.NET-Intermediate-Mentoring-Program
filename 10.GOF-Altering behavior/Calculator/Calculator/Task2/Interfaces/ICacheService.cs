namespace Calculator.Task2.Interfaces
{
    public interface ICacheService<T>
    {
        T Get(string key);
        void Set(string key, T value);
    }
}
