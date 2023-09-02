namespace VintageHub.Client.Pages.Admin;

public partial class CreateArtifact
{
    private const long MaxFileSize = 1024 * 1024 * 5; // represents 5MB
    private CreateArtifactModel artifact = new();
    private List<CategoryModel> categories;
    private List<EraModel> eras;
    private List<VendorDisplayModel> vendors;
    private VendorDisplayModel selectedVendor;
    private IBrowserFile selectedImageFile;
    private string errorMessage = "";
    private string imageSource = "";
    private bool isLoading = true;
    private bool isCreatingArtifact = false;
    private bool viewPreview = false;

    protected override async Task OnInitializedAsync()
    {
        vendors = await VendorEndpoint.GetAllVendorsAsync();
        categories = await CategoryEndpoint.GetAllCategoriesAsync();
        eras = await EraEndpoint.GetAllErasAsync();

        artifact.CategoryId = categories.FirstOrDefault().Id;
        artifact.EraId = eras.FirstOrDefault().Id;
        artifact.VendorId = vendors.FirstOrDefault().Id;
        isLoading = false;
    }

    private async Task<IEnumerable<VendorDisplayModel>> SearchVendorsAsync(string value)
    {
        var vendors = await VendorEndpoint.GetAllVendorsAsync();
        return vendors.Where(v => v.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private void ClosePage()
    {
        Navigation.NavigateTo("/");
    }

    private void LoadPreview()
    {
        viewPreview = true;
        selectedVendor = vendors.FirstOrDefault(v => v.Id == artifact.VendorId);
    }

    private async Task LoadImageSourceAsync()
    {
        if (selectedImageFile is null)
        {
            return;
        }

        using var stream = selectedImageFile.OpenReadStream(selectedImageFile.Size);
        using var memoryStream = new MemoryStream();

        await stream.CopyToAsync(memoryStream);
        byte[] bytes = memoryStream.ToArray();

        imageSource = $"data:{selectedImageFile.ContentType};base64,{Convert.ToBase64String(bytes)}";
    }

    private async Task HandleImageSelected(IBrowserFile e)
    {
        selectedImageFile = e;
        await LoadImageSourceAsync();
    }

    private bool IsArtifactInvalid(ArtifactModel artifact)
    {
        var selectedEra = eras.Where(e => e.Id == artifact.EraId).FirstOrDefault();
        var selectedCategory = categories.Where(c => c.Id == artifact.CategoryId).FirstOrDefault();

        if (selectedEra is null || selectedCategory is null)
        {
            errorMessage = Localizer["era-unavailable"];
            return true;
        }

        if (selectedCategory is null)
        {
            errorMessage = Localizer["category-unavailable"];
            return true;
        }

        if (selectedImageFile?.Size > MaxFileSize)
        {
            errorMessage = $"{Localizer["image-above-limit"]} 5MB";
            selectedImageFile = null;
            return true;
        }

        return false;
    }

    private async Task CreateArtifactAsync()
    {
        errorMessage = "";
        var newArtifact = new ArtifactModel
        {
            Name = artifact.Name,
            Description = artifact.Description,
            Quantity = artifact.Quantity,
            Price = artifact.Price,
            DiscountAmount = artifact.DiscountAmount,
            CategoryId = artifact.CategoryId,
            VendorId = artifact.VendorId,
            ImageId = "",
            EraId = artifact.EraId,
            Availability = artifact.Availability,
        };

        if (IsArtifactInvalid(newArtifact))
        {
            return;
        }

        isCreatingArtifact = true;
        if (selectedImageFile is not null)
        {
            string objectId = await ImageEndpoint.UploadImageAsync(selectedImageFile);
            newArtifact.ImageId = objectId;
        }

        await ArtifactEndpoint.InsertArtifactAsync(newArtifact);
        artifact = new();
        isCreatingArtifact = false;
        ClosePage();
    }
}