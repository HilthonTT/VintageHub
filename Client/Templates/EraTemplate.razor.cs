namespace VintageHub.Client.Templates;

public partial class EraTemplate
{
    [Parameter]
    [EditorRequired]
    public EraModel Era { get; set; }

    private string imageSource = "";
    private string truncatedName = "";
    private string truncatedDescription = "";

    protected override void OnInitialized()
    {
        TruncateTexts();
        imageSource = GetImageSource();
    }

    private void TruncateTexts()
    {
        string name = Era.Name;
        truncatedName = name?.Length > 20 ? string.Concat(name.AsSpan(0, 17), "...") : name;

        string description = Era.Description;
        truncatedDescription = description?.Length > 75 ? string.Concat(description.AsSpan(0, 72), "...") : description;
    }

    private string GetImageSource()
    {
        string source = Era.Name switch
        {
            "Victorian Era" => "victorian.jpg",
            "Roaring Twenties" => "roaring-twenties.jpg",
            "Art Deco Period" => "art-deco.jpg",
            "Renaissance Era" => "renaissance.jpg",
            "Ancient Civilizations" => "ancient-civilizations.jpg",
            "Industrial Revolution" => "industrial-revolution.jpg",
            "Space Age" => "space-age.jpg",
            "Medieval Period" => "medieval-period.jpg",
            _ => "",
        };

        return $"images/era/{source}";
    }

    private void OnEraClick()
    {
        var uriBuilder = new UriBuilder(Navigation.ToAbsoluteUri("/Listing"));
        var queryParameters = new Dictionary<string, string>
        {
            ["eraId"] = Era.Id.ToString()
        };

        string queryString = string.Join("&", queryParameters.Select(kv => $"{kv.Key}={kv.Value}"));
        uriBuilder.Query = queryString;

        Navigation.NavigateTo(uriBuilder.Uri.ToString());
    }
}