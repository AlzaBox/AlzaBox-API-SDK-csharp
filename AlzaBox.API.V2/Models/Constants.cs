namespace AlzaBox.API.V2.Models;

public static class Constants
{
    public const string ContentTypeFormUrlUncoded = "application/x-www-form-urlencoded";
    public static readonly string ScopeKonzoleAccess = "konzole_access";
    public static readonly string GrantTypePassword ="password";
    public const string TestIdentityBaseUrl = $"https://identitymanagement.phx-test.alza.cz/connect/token";
    public const string TestParcelLockersBaseUrl = $"https://parcellockersconnector-test.alza.cz/parcel-lockers/";
    public const string TestVirtualLockersUrl = $"https://plvirtualboxconnector-test.alza.cz";
}

public static class ReservationType
{
    public const string NonBinding = "NON_BINDING";
    public const string Immediate = "IMMEDIATE";
    public const string Time = "TIME";
}

public static class CourierAccessType
{
    public const string Full = "FULL";
    public const string Specific = "SPECIFIC";
}