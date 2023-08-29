namespace VintageHub.Client.Templates;

public partial class ArtifactCarouselTemplate
{
    [Parameter]
    [EditorRequired]
    public ArtifactDisplayModel Artifact { get; set; }

    private string imageSource = "";
    protected override void OnInitialized()
    {
        imageSource = ImageEndpoint.GetImage(Artifact.ImageId);
    }

    private void LoadArtifactPage()
    {
        Navigation.NavigateTo($"/Artifact/{Artifact.Id}");
    }
}