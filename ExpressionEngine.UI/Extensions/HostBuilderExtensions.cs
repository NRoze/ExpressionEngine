using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ExpressionEngine.UI.Extensions
{
    static public class HostBuilderExtensions
    {
        extension(WebAssemblyHostBuilder builder)
        {
            public IServiceCollection AddHttpClient()
            {
                var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

                if (string.IsNullOrWhiteSpace(apiBaseUrl))
                    throw new InvalidOperationException("ApiBaseUrl configuration is missing or empty.");

                return builder.Services.AddScoped(sp => new HttpClient
                {
                    BaseAddress = new Uri(apiBaseUrl)
                });
            }
        }
    }
}
