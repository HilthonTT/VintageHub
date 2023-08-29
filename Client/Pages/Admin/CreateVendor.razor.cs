namespace VintageHub.Client.Pages.Admin;

public partial class CreateVendor
{
    private const long MaxFileSize = 1024 * 1024 * 5; // represents 5MB
    private CreateVendorModel vendor = new();
    private List<UserModel> users;
    private IBrowserFile selectedImageFile;
    private string errorMessage = "";
    private string imageSource = "";
    private bool isCreatingVendor = false;
    protected override async Task OnInitializedAsync()
    {
        users = await UserEndpoint.GetAllUsersAsync();
    }

    private async Task<IEnumerable<UserModel>> SearchUserAsync(string value)
    {
        var users = await UserEndpoint.GetAllUsersAsync();

        return users.Where(
            u => u.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || 
            u.LastName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private void ClosePage()
    {
        Navigation.NavigateTo("/");
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

    private bool IsVendorInvalid(VendorModel vendor)
    {
        var selectedUser = users.Where(u => u.Id == vendor.OwnerUserId).FirstOrDefault();
        if (selectedUser is null)
        {
            errorMessage = Localizer["user-unavailable"];
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

    private async Task CreateVendorAsync()
    {
        errorMessage = "";
        var newVendor = new VendorModel
        {
            OwnerUserId = vendor.OwnerUserId,
            Name = vendor.Name,
            ImageId = "",
            Description = vendor.Description,
            DateFounded = vendor.DateFounded.GetValueOrDefault(),
        };

        if (IsVendorInvalid(newVendor))
        {
            return;
        }

        isCreatingVendor = true;
        if (selectedImageFile is not null)
        {
            string objectId = await ImageEndpoint.UploadImageAsync(selectedImageFile);
            newVendor.ImageId = objectId;
        }

        await VendorEndpoint.InsertVendorAsync(newVendor);
        vendor = new();
        isCreatingVendor = false;

        ClosePage();
    }
}