namespace VintageHub.Client.Templates;

public partial class CreatingTemplate
{
    [Parameter]
    [EditorRequired]
    public Item ItemType { get; set; }

    private string GetTitle()
    {
        return ItemType switch
        {
            Item.Artifact => Localizer["creating-artifact"],
            Item.Review => Localizer["creating-review"],
            Item.Vendor => Localizer["creating-vendor"],
            _ => "",
        };
    }

    private string GetDescription()
    {
        return ItemType switch
        {
            Item.Artifact => Localizer["creating-artifact-description"],
            Item.Review => Localizer["creating-review-description"],
            Item.Vendor => Localizer["creating-vendor-description"],
            _ => "",
        };
    }
}