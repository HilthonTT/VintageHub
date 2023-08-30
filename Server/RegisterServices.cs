namespace VintageHub.Server;

public static class RegisterServices
{
    private static void AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("jobTitle", "Admin");
            });
        });
    }

    private static void ConfigureMaxRequestSize(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        builder.Services.Configure<IISServerOptions>(options =>
        {
            options.MaxRequestBodySize = int.MaxValue;
        });
    }

    private static void ConfigureCors(this WebApplicationBuilder builder)
    {
        var allowedDomains = builder.Configuration.GetSection("AllowedDomains");

        var allowedDomainsDictionary = allowedDomains
            .GetChildren()
            .ToDictionary(x => x.Key, x => x.Value);

        builder.Services.AddCors(cors =>
        {
            cors.AddPolicy("AllowClientDomain", options =>
            {
                options.WithOrigins(allowedDomainsDictionary.Values.ToArray());
            });
        });
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddMemoryCache();

        builder.AddAuthentication();
        builder.ConfigureMaxRequestSize();
        builder.ConfigureCors();
        

        builder.Services.AddTransient<IMongoDbConnection, MongoDbConnection>();
        builder.Services.AddTransient<IImageData, ImageData>();

        builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddTransient<IArtifactData, ArtifactData>();
        builder.Services.AddTransient<ICategoryData, CategoryData>();
        builder.Services.AddTransient<IEraData, EraData>();
        builder.Services.AddTransient<IOrderData, OrderData>();
        builder.Services.AddTransient<IReviewData, ReviewData>();
        builder.Services.AddTransient<IUserData, UserData>();
        builder.Services.AddTransient<IVendorData, VendorData>();
        builder.Services.AddTransient<IWishlistData, WishlistData>();
    }
}
