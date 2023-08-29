namespace VintageHub.Client.Pages;

public partial class Index
{
    private List<ArtifactDisplayModel> artifacts;
    private List<CategoryModel> categories;
    private List<EraModel> eras;
    private string errorMessage = "";
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await UserVerifier.LoadAndVerifyUserAsync();
            await LoadArtifactsAsync();

            categories = await CategoryEndpoint.GetAllCategoriesAsync();
            eras = await EraEndpoint.GetAllErasAsync();
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
    }

    private void LoadListingPage()
    {
        Navigation.NavigateTo("/Listing");
    }

    private void LoadWishListPage()
    {
        Navigation.NavigateTo("/Wishlist");
    }

    private void LoadDiscountPage()
    {
        var uriBuilder = new UriBuilder(Navigation.ToAbsoluteUri("/Listing"));
        var queryParameters = new Dictionary<string, string>
        {
            ["filterDiscounts"] = true.ToString(),
        };

        string queryString = string.Join("&", queryParameters.Select(kv => $"{kv.Key}={kv.Value}"));
        uriBuilder.Query = queryString;

        Navigation.NavigateTo(uriBuilder.Uri.ToString());
    }

    private void LoadOrdersPage()
    {
        Navigation.NavigateTo("/Order/Previous");
    }

    private void LoadCustomerServicePage()
    {
        Navigation.NavigateTo("/CustomerServices");
    }

    private async Task LoadArtifactsAsync()
    {
        var rnd = new Random();
        var artifactList = await ArtifactEndpoint.GetAllArtifactsAsync();

        artifacts = artifactList.OrderBy(_ => rnd.Next()).Take(6).ToList();
    }

    private async Task LoadLanguageDialogAsync()
    {
        await DialogService.ShowAsync<LanguageSelector>();
    }
}