namespace Calculator.Task4.Interfaces
{
    public interface ICacheService<T>
    {
        T Get(string key);
        void Set(string key, T value);
    }
}
