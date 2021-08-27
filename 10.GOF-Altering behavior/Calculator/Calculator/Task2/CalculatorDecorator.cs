using Calculator.Task2.Interfaces;

namespace Calculator.Task2
{
    public abstract class CalculatorDecorator: ICalculator
    {
        protected ICalculator _calculator;

        protected CalculatorDecorator(ICalculator calculator)
        {
            _calculator = calculator;
        }

        public abstract decimal CalculatePayment(string touristName);
    }
}
