namespace VintageHub.Client.Dialog;

public partial class EditArtifact
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public ArtifactDisplayModel Artifact { get; set; }

    [Parameter]
    public UserModel LoggedInUser { get; set; }

    private const long MaxFileSize = 1024 * 1024 * 5; // represents 5MB
    private CreateArtifactModel model = new();
    private List<CategoryModel> categories;
    private List<EraModel> eras;
    private IBrowserFile selectedImageFile;
    private bool isAllowed = false;
    private bool isCurrentlyEditing = false;
    private string modelImageSource = "";
    private string imageSource = "";
    private string errorMessage = "";
    private DialogOptions options = new()
    {
        ClassBackground = "dialog-backdrop",
        CloseButton = true,
        CloseOnEscapeKey = true,
    };
    protected override async Task OnInitializedAsync()
    {
        isAllowed = IsAllowed();
        if (isAllowed is false)
        {
            Cancel();
        }

        imageSource = ImageEndpoint.GetImage(Artifact.ImageId);
        MapModelToArtifact();
        categories = await CategoryEndpoint.GetAllCategoriesAsync();
        eras = await EraEndpoint.GetAllErasAsync();
    }

    private async Task EditArtifactAsync()
    {
        if (isAllowed is false)
        {
            Snackbar.Add(Localizer["edit-no-permission"], Severity.Error);
            Cancel();
        }
        else
        {
            isCurrentlyEditing = true;
            MapArtifactToModel();
            var modifiedArtifact = new ArtifactModel(Artifact);

            if (IsArtifactInvalid(modifiedArtifact))
            {
                isCurrentlyEditing = false;
                return;
            }

            if (selectedImageFile?.Size > 0)
            {
                Artifact.ImageId = await ImageEndpoint.UploadImageAsync(selectedImageFile);
            }

            await ArtifactEndpoint.UpdateArtifactAsync(modifiedArtifact);

            isCurrentlyEditing = false;
            Snackbar.Add(Localizer["edit-artifact-sucessful"], Severity.Success);
            Cancel();
        }
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
        modelImageSource = $"data:{selectedImageFile.ContentType};base64,{Convert.ToBase64String(bytes)}";
    }

    private async Task HandleImageSelected(IBrowserFile e)
    {
        selectedImageFile = e;
        await LoadImageSourceAsync();
    }

    private void MapArtifactToModel()
    {
        Artifact.Name = model.Name;
        Artifact.Description = model.Description;
        Artifact.ImageId = model.ImageId;
        Artifact.Quantity = model.Quantity;
        Artifact.Price = model.Price;
        Artifact.DiscountAmount = model.DiscountAmount;
        Artifact.Category.Id = model.CategoryId;
        Artifact.Era.Id = model.EraId;
        Artifact.Availability = model.Availability;
    }

    private void MapModelToArtifact()
    {
        model.Name = Artifact.Name;
        model.Description = Artifact.Description;
        model.ImageId = Artifact.ImageId;
        model.Quantity = Artifact.Quantity;
        model.Price = Artifact.Price;
        model.DiscountAmount = Artifact.DiscountAmount;
        model.CategoryId = Artifact.Category.Id;
        model.EraId = Artifact.Era.Id;
        model.Availability = Artifact.Availability;
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

    private bool IsAllowed()
    {
        if (Artifact?.Vendor?.OwnerUserId == LoggedInUser?.Id)
        {
            return true;
        }

        return false;
    }

    private void Cancel()
    {
        MudDialog?.Close();
    }
}