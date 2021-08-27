using Calculator.Task4.Interfaces;

namespace Calculator.Task4.Calculators
{
    public class LoggingInsurancePaymentCalculator : CalculatorDecorator
    {
        private readonly ILogger _logger;

        public LoggingInsurancePaymentCalculator(ILogger logger, ICalculator calculator) : base(calculator)
        {
            _logger = logger;
        }

        public override decimal CalculatePayment(string touristName)
        {
            _logger.Log("Start");

            var payment = _calculator.CalculatePayment(touristName);

            _logger.Log("End");

            return payment;
        }
    }
}
