using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Parametriz.AutoNFP.ConsoleApp.Configs;

var builder = Host.CreateDefaultBuilder(args);

builder
    .AddAppConfiguration()
    .UseEnvironment(ObterEnvironment())
    .AddDatabaseConfiguration()
    .AddDependencyInjectionConfiguration()
    .Build()
    .Run();

string ObterEnvironment()
{
#if DEBUG
    return Environments.Development;
#else
    return Environments.Production;
#endif
}