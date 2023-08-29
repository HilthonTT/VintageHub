namespace VintageHub.Client.Pages.Admin;

public partial class Admin
{
    private List<ArtifactDisplayModel> artifacts;
    protected override async Task OnInitializedAsync()
    {
        var rnd = new Random();
        var artifactList = await ArtifactEndpoint.GetAllArtifactsAsync();
        artifacts = artifactList.OrderBy(_ => rnd.Next()).ToList();
    }

    private void LoadCreateArtifactPage()
    {
        Navigation.NavigateTo("/Admin/Artifact/Create");
    }

    private void LoadCreateVendorPage()
    {
        Navigation.NavigateTo("/Admin/Vendor/Create");
    }

    private void LoadOrderListingPage()
    {
        Navigation.NavigateTo("/Admin/OrderListing");
    }
}