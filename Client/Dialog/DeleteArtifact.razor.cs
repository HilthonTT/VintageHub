namespace VintageHub.Client.Dialog;

public partial class DeleteArtifact
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public ArtifactDisplayModel Artifact { get; set; }

    [Parameter]
    public UserModel LoggedInUser { get; set; }

    private bool isAllowed = false;
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

    private async Task DeleteArtifactAsync()
    {
        if (isAllowed is false)
        {
            Snackbar.Add(Localizer["delete-no-permission"], Severity.Error);
            Cancel();
        }
        else
        {
            await ArtifactEndpoint.DeleteArtifactAsync(new ArtifactModel(Artifact));
            Snackbar.Add(Localizer["delete-artifact-successfull"], Severity.Success);
            ClosePage();
        }
    }

    private bool IsAllowed()
    {
        if (Artifact?.Vendor.OwnerUserId == LoggedInUser?.Id)
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