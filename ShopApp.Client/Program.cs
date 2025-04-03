using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopApp.UseCases.Services.Category;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();


builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7027/api/")
});

await builder.Build().RunAsync();
