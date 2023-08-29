namespace VintageHub.Client.Templates;

public partial class CategoryTemplate
{
    [Parameter]
    [EditorRequired]
    public CategoryModel Category { get; set; }

    private string imageSource = "";
    protected override void OnInitialized()
    {
        imageSource = GetImageSource();
    }

    private string GetImageSource()
    {
        string source = Category.Name switch
        {
            "Jewelry and Accessories" => "jewelry.jpg",
            "Home Decor" => "home-decor.jpg",
            "Collectibles" => "collectibles.jpg",
            "Fashion and Apparel" => "fashion.jpg",
            "Art and Paintings" => "art.jpg",
            "Ceramics and Glassware" => "glassware.jpg",
            "Watches and Clocks" => "watches.jpg",
            "Technology and Gadgets" => "technology.jpg",
            "Automobile and Transportation" => "automobile.jpg",
            "Sports and Recreation" => "recreation.jpg",
            _ => "",
        };
        return $"images/category/{source}";
    }

    private void OnCategoryClick()
    {
        var uriBuilder = new UriBuilder(Navigation.ToAbsoluteUri("/Listing"));
        var queryParameters = new Dictionary<string, string>
        {
            ["categoryId"] = Category.Id.ToString()
        };

        string queryString = string.Join("&", queryParameters.Select(kv => $"{kv.Key}={kv.Value}"));
        uriBuilder.Query = queryString;

        Navigation.NavigateTo(uriBuilder.Uri.ToString());
    }
}