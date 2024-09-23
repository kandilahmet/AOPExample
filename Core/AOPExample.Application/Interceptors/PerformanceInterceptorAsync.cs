using AOPExample.Application.Attributes;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPExample.Application.Interceptors
{
    public class PerformanceInterceptorAsync : InterceptorBaseAsync<PerformanceAttribute>
    {
        private readonly ILogger<PerformanceInterceptorAsync> _logger;
        private readonly Stopwatch _stopwatch;
        public PerformanceInterceptorAsync(ILogger<PerformanceInterceptorAsync> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        protected override void OnBefore(IInvocation invocation, PerformanceAttribute attribute)
        {
            _stopwatch.Start();
            _logger.LogInformation($"{invocation.Method.Name} Start");
        }

        protected override void OnAfter(IInvocation invocation, PerformanceAttribute attribute)
        {
            if (_stopwatch.Elapsed.TotalMilliseconds > attribute.Interval)
            {
                _logger.LogInformation($"{invocation.Method.Name} elapsed {_stopwatch.Elapsed.TotalSeconds} second(s).");
            }

            _stopwatch.Stop();
            _stopwatch.Reset();

        }
        protected override void OnException(IInvocation invocation, Exception ex, PerformanceAttribute attribute)
        {
            _logger.LogWarning($"{invocation.Method.Name} ExceptionMessage : {ex.Message}");
        }
    }
}
