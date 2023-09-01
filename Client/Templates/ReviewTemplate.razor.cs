namespace VintageHub.Client.Templates;

public partial class ReviewTemplate
{
    [Parameter]
    [EditorRequired]
    public ReviewDisplayModel Review { get; set; }

    [Parameter]
    [EditorRequired]
    public ArtifactDisplayModel Artifact { get; set; }

    [Parameter]
    [EditorRequired]
    public UserModel LoggedInUser { get; set; }

    private string reviewerFullName = "";

    protected override void OnInitialized()
    {
        reviewerFullName = $"{Review.User?.FirstName} {Review.User?.LastName}";
    }

    private bool IsAuthor()
    {
        if (LoggedInUser?.Id == Review.User.Id)
        {
            return true;
        }

        return false;
    }

    private async Task SendEditRequestAsync()
    {
        if (IsAuthor() is false)
        {
            Snackbar.Add("You do not have permission to edit this review");
        }
        else
        {
            var parameters = new DialogParameters<EditReview>()
            {
                { x => x.Review, Review },
                { x => x.LoggedInUser, LoggedInUser },
            };

            await DialogService.ShowAsync<EditReview>("Edit Review", parameters);
        }
    }

    private void LoadReviewPage()
    {
        Navigation.NavigateTo($"/Artifact/{Artifact.Id}/Review/{Review.Id}");
    }
}