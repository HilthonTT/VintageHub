namespace VintageHub.Client.Dialog;

public partial class EditReview
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public ReviewDisplayModel Review { get; set; }

    [Parameter]
    public UserModel LoggedInUser { get; set; }

    private CreateReviewModel model = new();
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

        model.Title = Review.Title;
        model.Description = Review.Description;
        model.Rating = Review.Rating;
    }

    private async Task EditReviewAsync()
    {
        if (isAllowed is false)
        {
            Snackbar.Add(Localizer["edit-no-permission"], Severity.Error);
            Cancel();
        }
        else
        {
            Review.Title = model.Title;
            Review.Description = model.Description;
            Review.Rating = model.Rating;
            await ReviewEndpoint.UpdateReviewAsync(new ReviewModel(Review));
            Snackbar.Add(Localizer["edit-review-successful"], Severity.Success);
            Cancel();
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