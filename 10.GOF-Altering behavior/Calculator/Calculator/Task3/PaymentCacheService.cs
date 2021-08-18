using System;
using System.Collections.Generic;
using Calculator.Task3.Interfaces;

namespace Calculator.Task3
{
    public class PaymentCacheService: ICacheService<decimal?>
    {
        private readonly IDictionary<string, decimal> _paymentCache;

        public PaymentCacheService()
        {
            _paymentCache = new Dictionary<string, decimal>();
        }

        public decimal? Get(string key)
        {
            if (_paymentCache.TryGetValue(key, out var payment))
            {
                return payment;
            }

            return null;
        }

        public void Set(string key, decimal? value)
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (_paymentCache.TryGetValue(key, out _))
            {
                return;
            }

            _paymentCache.Add(key, value.Value);
        }
    }
}
