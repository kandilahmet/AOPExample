using AOPExample.Application;
using AOPExample.Infrastructure;
using AOPExample.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var hostBuilder = Host.CreateDefaultBuilder(args);
//hostBuilder.AddSerilogConfiguration();
var host = hostBuilder.ConfigureServices((hostContext, services) =>
{
    services.AddApplicationServices();
    services.AddInfrastructureServices();

})
.Build();

var myClass = host.Services.GetRequiredService<IMyClass>();

int a = 5, b = 6;

 await myClass.MultiplyAsync(a, b);
//myClass.Add(a, b);

Console.ReadLine();