using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Abstraction.Interfaces;
using Services.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication() // Auto-discovers bindings, including Timer and Http triggers.
    .ConfigureServices(services =>
    {
        services.AddHttpClient<IWeatherService, WeatherService>();
        services.AddSingleton<IStorageService, StorageService>();
    })
    .Build();

host.Run();