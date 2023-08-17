using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VintageHub.Client;
using Blazored.LocalStorage;
using Client.Library.Endpoints.Interfaces;
using Client.Library.Endpoints;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.LocalStorage;
using VintageHub.Client.Authentication.Interfaces;
using VintageHub.Client.Authentication;
using Blazored.SessionStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("VintageHub.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("VintageHub.ServerAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://TimTanAuth.onmicrosoft.com/f278b08e-7802-46cc-971e-a89fd6a8dd64/api_access");

    options.ProviderOptions.DefaultAccessTokenScopes.Add("openid profile");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");

    options.ProviderOptions.LoginMode = "redirect";
});

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireClaim("jobTitle", "Admin");
    });
});

builder.Services.AddTransient<IUserDataVerifier, UserDataVerifier>();

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddBlazoredSessionStorageAsSingleton();
builder.Services.AddSingleton<ILocalStorage, LocalStorage>();

builder.Services.AddSingleton<IShoppingCartStorage, ShoppingCartStorage>();

builder.Services.AddTransient<IImageEndpoint, ImageEndpoint>();
builder.Services.AddTransient<IArtifactEndpoint, ArtifactEndpoint>();
builder.Services.AddTransient<ICategoryEndpoint, CategoryEndpoint>();
builder.Services.AddTransient<IEraEndpoint, EraEndpoint>();
builder.Services.AddTransient<IOrderEndpoint, OrderEndpoint>();
builder.Services.AddTransient<IReviewEndpoint, ReviewEndpoint>();
builder.Services.AddTransient<IUserEndpoint, UserEndpoint>();
builder.Services.AddTransient<IVendorEndpoint, VendorEndpoint>();

await builder.Build().RunAsync();
