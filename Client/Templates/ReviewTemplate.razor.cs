namespace VintageHub.Client.Templates;

public partial class ReviewTemplate
{
    [Parameter]
    [EditorRequired]
    public ReviewDisplayModel Review { get; set; }

    [Parameter]
    [EditorRequired]
    public ArtifactDisplayModel Artifact { get; set; }

    private string reviewerFullName = "";

    protected override void OnInitialized()
    {
        reviewerFullName = $"{Review.User?.FirstName} {Review.User?.LastName}";
    }

    private void LoadReviewPage()
    {
        Navigation.NavigateTo($"/Artifact/{Artifact.Id}/Review/{Review.Id}");
    }
}