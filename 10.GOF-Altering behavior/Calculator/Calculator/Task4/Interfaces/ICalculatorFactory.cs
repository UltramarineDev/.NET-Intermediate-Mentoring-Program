namespace Calculator.Task4.Interfaces
{
    public interface ICalculatorFactory
    {
        ICalculator CreateCalculator();

        ICalculator CreateCalculator(bool withLogging, bool withCaching, bool withRounding);
    }
}
