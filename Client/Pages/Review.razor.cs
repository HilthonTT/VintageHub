namespace VintageHub.Client.Pages;

public partial class Review
{
    [Parameter]
    public int ArtifactId { get; set; }

    [Parameter]
    public int ReviewId { get; set; }

    private ReviewDisplayModel review;
    private bool isLoading = true;
    private string writerFullName = "";

    protected override async Task OnInitializedAsync()
    {
        review = await ReviewEndpoint.GetReviewByIdAsync(ReviewId);
        writerFullName = $"{review.User?.FirstName} {review.User?.LastName}";
        isLoading = false;
    }

    private void ClosePage()
    {
        Navigation.NavigateTo($"/Artifact/{ArtifactId}");
    }
}