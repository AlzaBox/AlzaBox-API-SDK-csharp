using System.Configuration;

namespace AlzaBox.API.Models;

public static class Constants
{
    public static readonly string ContentTypeFormUrlUncoded = "application/x-www-form-urlencoded";
    public static readonly string ScopeKonzoleAccess = "konzole_access";
    public static readonly string GrantTypePassword ="password";
    public static readonly string ProductionIdentityBaseUrl = $"https://identitymanagement.phx-test.alza.cz/connect/token";
    public static readonly string TestIdentityBaseUrl = $"https://identitymanagement.phx-test.alza.cz/connect/token";
    
    public static readonly string ProductionParcelLockersBaseUrl = $"https://parcellockersconnector-test.alza.cz/parcel-lockers/v1/";
    public static readonly string TestParcelLockersBaseUrl = $"https://parcellockersconnector-test.alza.cz/parcel-lockers/v1/";
}