using Calculator.Task3.Interfaces;

namespace Calculator.Task3.Calculators
{
    public class CachedInsurancePaymentCalculator : CalculatorDecorator
    {
        private readonly ICacheService<decimal?> _cacheService;

        public CachedInsurancePaymentCalculator(ICalculator calculator): base(calculator)
        {
            _cacheService = new PaymentCacheService();
        }

        public override decimal CalculatePayment(string touristName)
        {
            var cachedPayment = _cacheService.Get(touristName);
            if (cachedPayment.HasValue)
            {
                return cachedPayment.Value;
            }

            var payment = _calculator.CalculatePayment(touristName);

            _cacheService.Set(touristName, payment);

            return payment;
        }
    }
}
