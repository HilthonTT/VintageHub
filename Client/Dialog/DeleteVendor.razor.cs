namespace VintageHub.Client.Dialog;

public partial class DeleteVendor
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public VendorDisplayModel Vendor { get; set; }

    [Parameter]
    public UserModel LoggedInUser { get; set; }

    private bool isAllowed = false;
    private string typedVendorName = "";
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
    }

    private async Task DeleteVendorAsync()
    {
        if (CanNotDelete())
        {
            return;
        }

        if (isAllowed is false)
        {
            Snackbar.Add("You do have not have permission to delete the vendor.", Severity.Error);
            Cancel();
        }
        else
        {
            await VendorEndpoint.DeleteVendorAsync(new VendorModel(Vendor));
            Snackbar.Add($"Successfully deleted vendor {Vendor.Id}", Severity.Success);
            ClosePage();
        }
    }

    private bool CanNotDelete()
    {
        if (string.IsNullOrWhiteSpace(typedVendorName) || typedVendorName == Vendor?.Name)
        {
            errorMessage = "The vendor name you've provided is incorrect.";
            return true;
        }

        return false;
    }

    private bool IsAllowed()
    {
        if (Vendor?.Owner?.Id == LoggedInUser?.Id)
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