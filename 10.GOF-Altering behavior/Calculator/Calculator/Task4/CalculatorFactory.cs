using Calculator.Task4.Calculators;
using Calculator.Task4.Interfaces;

namespace Calculator.Task4
{
    public class CalculatorFactory : ICalculatorFactory
    {
        private ICurrencyService currencyService;
        private ITripRepository tripRepository;
        private ILogger logger;

        public CalculatorFactory(
            ICurrencyService currencyService,
            ITripRepository tripRepository,
            ILogger logger)
        {
            this.currencyService = currencyService;
            this.tripRepository = tripRepository;
            this.logger = logger;
        }

        public ICalculator CreateCalculator()
        {
            return new InsurancePaymentCalculator(currencyService, tripRepository);
        }

        public ICalculator CreateCalculator(bool withLogging, bool withCaching, bool withRounding)
        {
            ICalculator calculator = new InsurancePaymentCalculator(currencyService, tripRepository);

            if (withLogging)
            {
                calculator = new LoggingInsurancePaymentCalculator(logger, calculator);
            }

            if (withCaching)
            {
                calculator = new CachedInsurancePaymentCalculator(calculator);
            }

            if (withRounding)
            {
                calculator = new RoundingInsurancePaymentCalculator(calculator);
            }

            return calculator;
        }
    }
}
