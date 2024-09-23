using AOPExample.Application.Abstractions;
using AOPExample.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPExample.Infrastructure
{
    public  static class ServiceRegistation
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddInterceptedScopedAsync<IMyClass,MyClass>();
        }
    }
}
