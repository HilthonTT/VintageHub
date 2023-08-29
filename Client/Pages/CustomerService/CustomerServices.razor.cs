namespace VintageHub.Client.Pages.CustomerService;

public partial class CustomerServices
{
    private void ClosePage()
    {
        Navigation.NavigateTo("/");
    }

    private void CallSupport()
    {
        Navigation.NavigateTo("/CustomerServices/Call-Support");
    }
}