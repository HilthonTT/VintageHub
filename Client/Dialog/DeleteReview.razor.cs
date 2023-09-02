namespace VintageHub.Client.Dialog;

public partial class DeleteReview
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public ReviewDisplayModel Review { get; set; }

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

    private async Task DeleteReviewAsync()
    {
        if (isAllowed is false)
        {
            Snackbar.Add(Localizer["delete-no-permission"], Severity.Error);
            Cancel();
        }
        else
        {
            await ReviewEndpoint.DeleteReviewAsync(new ReviewModel(Review));
            Snackbar.Add(Localizer["delete-review-successful"], Severity.Success);
            ClosePage();
        }
    }

    private bool IsAllowed()
    {
        if (Review?.User.Id == LoggedInUser?.Id)
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