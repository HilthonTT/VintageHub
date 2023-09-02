namespace VintageHub.Client.Dialog;

public partial class EditVendor
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public VendorDisplayModel Vendor { get; set; }

    [Parameter]
    public UserModel LoggedInUser { get; set; }

    private const long MaxFileSize = 1024 * 1024 * 5; // represents 5MB
    private CreateVendorModel model = new();
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

    protected override void OnInitialized()
    {
        isAllowed = IsAllowed();
        if (isAllowed is false)
        {
            Cancel();
        }

        imageSource = ImageEndpoint.GetImage(Vendor.ImageId);
        MapModelToVendor();
    }

    private async Task EditVendorAsync()
    {
        if (isAllowed is false)
        {
            Snackbar.Add(Localizer["edit-no-permission"], Severity.Error);
            Cancel();
        }
        else
        {
            isCurrentlyEditing = true;
            MapVendorToModel();
            var modifiedVendor = new VendorModel(Vendor);

            if (IsVendorInvalid(modifiedVendor))
            {
                isCurrentlyEditing = false;
                return;
            }

            if (selectedImageFile?.Size > 0)
            {
                Vendor.ImageId = await ImageEndpoint.UploadImageAsync(selectedImageFile);
            }

            await VendorEndpoint.UpdateVendorAsync(modifiedVendor);
            
            isCurrentlyEditing = false;
            Snackbar.Add(Localizer["edit-vendor-successful"], Severity.Success);
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

    private void MapVendorToModel()
    {
        Vendor.Owner.Id = model.OwnerUserId;
        Vendor.Name = model.Name;
        Vendor.Description = model.Description;
        Vendor.DateFounded = model.DateFounded.GetValueOrDefault();
    }

    private void MapModelToVendor()
    {
        model.OwnerUserId = Vendor.Owner.Id;
        model.Name = Vendor.Name;
        model.Description = Vendor.Description;
        model.DateFounded = Vendor.DateFounded;
    }

    private bool IsVendorInvalid(VendorModel vendor)
    {
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
        if (Vendor?.Owner.Id == LoggedInUser?.Id)
        {
            return true;
        }

        return false;
    }

    private void Cancel()
    {
        MudDialog?.Close();
    }

    private void ClosePage()
    {
        Navigation.NavigateTo("/");
    }
}