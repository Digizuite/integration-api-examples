using Digizuite.Logging;
using DigizuiteClientExample;
using DigizuiteClientExample.Digizuite;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(Startup))]

namespace DigizuiteClientExample
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(typeof(ILogger<>), typeof(ConsoleLogger<>));
            builder.Services.AddSingleton<IDigizuiteClient, DigizuiteClient>();
        }
    }
}
