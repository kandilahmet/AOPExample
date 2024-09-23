using AOPExample.Application.Attributes;
using AOPExample.Application.Interceptors;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPExample.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IProxyGenerator, ProxyGenerator>();
            services.AddTransient<InterceptorBaseAsync<PerformanceAttribute>, PerformanceInterceptorAsync>();
            services.AddTransient<InterceptorBaseAsync<Performance2Attribute>, Performance2InterceptorAsync>();
        }

        #region AsyncInterceptor
        private static IAsyncInterceptor[]? GetAsyncInterceptors<TImplementation>(this IServiceProvider serviceProvider) where TImplementation : class
        {
            var classAttributes = typeof(TImplementation).GetCustomAttributes(typeof(AttributeBase), true).Cast<AttributeBase>();
            var methodAttributes = typeof(TImplementation).GetMethods().SelectMany(s => s.GetCustomAttributes(typeof(AttributeBase), true).Cast<AttributeBase>());

            var attributes = classAttributes.Union(methodAttributes).OrderBy(o => o.Priority);

            var interceptors = attributes.Select(f => serviceProvider.GetRequiredService(typeof(InterceptorBaseAsync<>).MakeGenericType(f.GetType()))).Cast<IAsyncInterceptor>();

            return interceptors.ToArray();
        }

        public static IServiceCollection AddInterceptedScopedAsync<TInterface, TImplementation>(this IServiceCollection serviceCollection)
     where TInterface : class where TImplementation : class, TInterface
        {
            serviceCollection.AddScoped<TImplementation>();
            serviceCollection.AddScoped(typeof(TInterface), serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<IProxyGenerator>();
                var implementation = serviceProvider.GetRequiredService<TImplementation>();

                IAsyncInterceptor[]? interceptors = serviceProvider.GetAsyncInterceptors<TImplementation>();
                // selector kullanacaksak
                var _selector = new InterceptorSelectorAsync<TImplementation>();
                var options = new ProxyGenerationOptions() { Selector = _selector };//Seçilen Interceptor'leri veriyoruz. null olursa varsayılan davranışı gösterir.

                return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(implementation, options, interceptors);

                // selector kullanmayacaksak
                // return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>( target : implementation, interceptors: interceptors);

            });

            return serviceCollection;
        }
        #endregion AsyncInterceptor
    }
}
