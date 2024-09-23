using AOPExample.Application.Abstractions;
using AOPExample.Application.Attributes;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPExample.Infrastructure
{
    [Performance(Interval = 2, Priority = 2)]
    [Performance2(Interval = 2, Priority = 1)]
    class MyClass : IMyClass
    {
        ILogger<MyClass> _logger;
        public MyClass(ILogger<MyClass> logger)
        {
            _logger = logger;
        }
        public async Task<int> MultiplyAsync(int num1, int num2)
        {
            _logger.LogInformation("MultiplyAsync Baslasdi");
            await Task.Delay(7000);
            _logger.LogInformation("MultiplyAsync Bitti");
            return num1 * num2;
        }

        public int Add(int num1, int num2)
        {
            _logger.LogInformation("Add Baslasdi");
            Task.Delay(2000);
            _logger.LogInformation("Add Bitti");
            return num1 + num2;
        }
    }
}
