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
    public class Performance2InterceptorAsync : InterceptorBaseAsync<Performance2Attribute>
    {
        private readonly ILogger<Performance2InterceptorAsync> _logger;
        private readonly Stopwatch _stopwatch;
        public Performance2InterceptorAsync(ILogger<Performance2InterceptorAsync> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        protected override void OnBefore(IInvocation invocation, Performance2Attribute attribute)
        {
            _stopwatch.Start();
            _logger.LogInformation($"{invocation.Method.Name} Start");
        }

        protected override void OnAfter(IInvocation invocation, Performance2Attribute attribute)
        {
            if (_stopwatch.Elapsed.TotalMilliseconds > attribute.Interval)
            {
                _logger.LogInformation($"{invocation.Method.Name} elapsed {_stopwatch.Elapsed.TotalSeconds} second(s).");
            }

            _stopwatch.Stop();
            _stopwatch.Reset();

        }
        protected override void OnException(IInvocation invocation, Exception ex, Performance2Attribute attribute)
        {
            _logger.LogWarning($"{invocation.Method.Name} ExceptionMessage : {ex.Message}");
        }
    }
}
