namespace VintageHub.Client.Shared.Authentication;

public partial class AuthView
{
    [Parameter]
    [EditorRequired]
    public AuthState AuthState { get; set; }

    private string GetTitle()
    {
        return AuthState switch
        {
            AuthState.CompletingLogin => Localizer["auth-completing-login"],
            AuthState.CompletingLogout => Localizer["auth-completing-logout"],
            AuthState.Logging => Localizer["auth-logging"],
            AuthState.LoginFailed => Localizer["auth-login-failed"],
            AuthState.Logout => Localizer["auth-logout"],
            AuthState.LogoutFailed => Localizer["auth-logout-failed"],
            AuthState.LogoutSuceeded => Localizer["auth-logout-suceeded"],
            AuthState.Registering => Localizer["auth-registering"],
            _ => Localizer["error"],
        };
    }

    private string GetDescription()
    {
        return AuthState switch
        {
            AuthState.CompletingLogin => Localizer["auth-completing-login-description"],
            AuthState.CompletingLogout => Localizer["auth-completing-logout-description"],
            AuthState.Logging => Localizer["auth-logging-description"],
            AuthState.LoginFailed => Localizer["auth-login-failed-description"],
            AuthState.Logout => Localizer["auth-logout-description"],
            AuthState.LogoutFailed => Localizer["auth-logout-failed-description"],
            AuthState.LogoutSuceeded => Localizer["auth-logout-suceeded-description"],
            AuthState.Registering => Localizer["auth-registering-description"],
            _ => Localizer["error-description"]
        };
    }

    private string GetIcon()
    {
        return AuthState switch
        {
            AuthState.CompletingLogin => Icons.Material.Filled.Computer,
            AuthState.CompletingLogout => Icons.Material.Filled.Computer,
            AuthState.Logging => Icons.Material.Filled.Login,
            AuthState.LoginFailed => Icons.Material.Filled.Error,
            AuthState.Logout => Icons.Material.Filled.Logout,
            AuthState.LogoutFailed => Icons.Material.Filled.Error,
            AuthState.LogoutSuceeded => Icons.Material.Filled.Logout,
            AuthState.Registering => Icons.Material.Filled.AppRegistration,
            _ => Icons.Material.Filled.QuestionMark,
        };
    }

    private Color GetColor()
    {
        return AuthState switch
        {
            AuthState.LoginFailed => Color.Error,
            AuthState.LogoutFailed => Color.Error,
            _ => Color.Primary,
        };
    }
}