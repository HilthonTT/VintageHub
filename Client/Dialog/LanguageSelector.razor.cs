namespace VintageHub.Client.Dialog;

public partial class LanguageSelector
{
    private DialogOptions options = new()
    {
        ClassBackground = "dialog-backdrop",
        CloseButton = true,
        CloseOnEscapeKey = true,
    };

    private List<CultureInfo> cultures = new()
    {
        new("en-US"),
        new("fr-FR"),
    };
    private CultureInfo culture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            if (CultureInfo.CurrentCulture != value)
            {
                var js = (IJSInProcessRuntime)JSRuntime;
                js.InvokeVoid("blazorCulture.set", value.Name);
                Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
            }
        }
    }
}