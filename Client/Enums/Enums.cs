namespace VintageHub.Client.Enums;

public enum Rating
{
    ZeroStar,
    OneStar,
    TwoStar,
    ThreeStar,
    FourStar,
    FiveStar
}

public enum AuthState
{
    CompletingLogin,
    CompletingLogout,
    Logging,
    LoginFailed,
    Logout,
    LogoutFailed,
    LogoutSuceeded,
    Registering
}