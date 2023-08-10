namespace VintageHub.Server;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.AddMemoryCache();

        builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddTransient<IArtifactData, ArtifactData>();
        builder.Services.AddTransient<ICategoryData, CategoryData>();
        builder.Services.AddTransient<IEraData, EraData>();
        builder.Services.AddTransient<IOrderData, OrderData>();
        builder.Services.AddTransient<IReviewData, ReviewData>();
        builder.Services.AddTransient<IUserData, UserData>();
        builder.Services.AddTransient<IVendorData, VendorData>();
    }
}
