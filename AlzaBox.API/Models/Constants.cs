namespace AlzaBox.API.Models;

public static class Constants
{
    public const string ContentTypeFormUrlUncoded = "application/x-www-form-urlencoded";
    public static readonly string ScopeKonzoleAccess = "konzole_access";
    public static readonly string GrantTypePassword ="password";
    public const string TestIdentityBaseUrl = $"https://identitymanagement.phx-test.alza.cz/connect/token";
    public const string TestParcelLockersBaseUrl = $"https://parcellockersconnector-test.alza.cz/parcel-lockers/v1/";
}