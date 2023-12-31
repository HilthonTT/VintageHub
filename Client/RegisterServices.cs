﻿namespace VintageHub.Client;

public static class RegisterServices
{
    private static string GetAuthenticationPath(WebAssemblyHostBuilder builder, string policyId)
    {
        string clientId = builder.Configuration.GetValue<string>("AzureAdB2C:ClientId");
        string callbackPath = builder.Configuration.GetValue<string>("AzureAdB2C:CallbackPath");
        string instance = builder.Configuration.GetValue<string>("AzureAdB2C:Instance");
        string domain = builder.Configuration.GetValue<string>("AzureAdB2C:Domain");

        string policy = builder.Configuration.GetValue<string>(policyId);

        return $"{instance}" +
                $"{domain}/oauth2/v2.0/authorize?p={policy}" +
                $"&client_id={clientId}&nonce=defaultNonce" +
                $"&redirect_uri={HttpUtility.UrlEncode(callbackPath)}" +
                $"&scope=openid&response_type=code&prompt=login" +
                $"&code_challenge_method=S256&code_challenge=codespecifictomyapp";
    }

    private static string GetDefaultAccessTokenScope(WebAssemblyHostBuilder builder)
    {
        return builder.Configuration.GetValue<string>("AzureAdB2C:DefaultScope");
    }

    public static void ConfigureAuthentication(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddMsalAuthentication(options =>
        {
            builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);

            options.ProviderOptions.DefaultAccessTokenScopes.Add(GetDefaultAccessTokenScope(builder));
            options.ProviderOptions.LoginMode = "redirect";

            options.AuthenticationPaths.RemoteProfilePath = GetAuthenticationPath(builder, "AzureAdB2C:EditProfilePolicyId");
        });

        builder.Services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("jobTitle", "Admin");
            });
        });
    }

    public static void ConfigureServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddLocalization();
        builder.Services.AddMudServices();

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
        builder.Services.AddTransient<IWishlistEndpoint, WishlistEndpoint>();
    }

    public async static Task SetDefaultCultureAsync(this WebAssemblyHost host)
    {
        var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
        var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");
        CultureInfo culture;

        if (result is not null)
        {
            culture = new CultureInfo(result);
        }
        else
        {
            culture = new CultureInfo("en-US");
        }
            
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
