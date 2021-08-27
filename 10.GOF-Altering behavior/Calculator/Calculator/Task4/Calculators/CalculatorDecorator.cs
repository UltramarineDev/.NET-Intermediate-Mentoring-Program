using Calculator.Task4.Interfaces;

namespace Calculator.Task4.Calculators
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
