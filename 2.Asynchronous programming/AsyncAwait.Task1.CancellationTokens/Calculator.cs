using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
    public static class Calculator
    {
        public static Task<long> Calculate(int n, CancellationToken cancellationToken = default)
        {
            long sum = 0;

            for (var i = 0; i < n; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                   cancellationToken.ThrowIfCancellationRequested();
                }

                // i + 1 is to allow 2147483647 (Max(Int32)) 
                sum += (i + 1);
                Thread.Sleep(10);
            }

            return Task.FromResult(sum);
        }
    }
}
