using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using demographic_data;
using System;
using System.Reflection;
using Microsoft.AspNetCore.Components.QuickGrid;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5201/") });

builder.Services.AddQuickGridEntityFrameworkAdapter();

await builder.Build().RunAsync();
