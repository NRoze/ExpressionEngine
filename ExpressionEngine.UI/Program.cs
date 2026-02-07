using ExpressionEngine.UI;
using ExpressionEngine.UI.Extensions;
using ExpressionEngine.UI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.AddHttpClient(); 

builder.Services.AddSingleton<BackendStatus>();

await builder.Build().RunAsync();
