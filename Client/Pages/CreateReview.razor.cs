namespace VintageHub.Client.Pages;

public partial class CreateReview
{
    [Parameter]
    public int Id { get; set; }

    private CreateReviewModel review = new();
    private UserModel loggedInUser;
    private ArtifactDisplayModel artifact;
    private string errorMessage = "";
    private string imageSource = "";
    private bool isCreatingReview = false;
    private bool noArtifact = false;
    protected override async Task OnInitializedAsync()
    {
        artifact = await ArtifactEndpoint.GetArtifactByIdAsync(Id);
        if (artifact is null)
        {
            noArtifact = true;
            return;
        }

        loggedInUser = await AuthProvider.GetUserFromAuth(UserEndpoint);
        imageSource = ImageEndpoint.GetImage(artifact.ImageId);
    }

    private void ClosePage()
    {
        Navigation.NavigateTo("/");
    }

    private void RedirectArtifactPage()
    {
        Navigation.NavigateTo($"/Artifact/{Id}");
    }

    private bool IsReviewInvalid()
    {
        if (artifact is null)
        {
            errorMessage = Localizer["artifact-unavailable"];
            return true;
        }

        if (loggedInUser is null)
        {
            errorMessage = Localizer["not-logged-in-make-review"];
            return true;
        }

        return false;
    }

    private async Task CreateReviewAsync()
    {
        errorMessage = "";
        var newReview = new ReviewModel
        {
            ArtifactId = artifact.Id,
            UserId = loggedInUser.Id,
            Title = review.Title,
            Description = review.Description,
            Rating = review.Rating,
        };

        if (IsReviewInvalid())
        {
            return;
        }

        isCreatingReview = true;
        await ReviewEndpoint.InsertReviewAsync(newReview);

        review = new();
        isCreatingReview = false;
        RedirectArtifactPage();
    }
}