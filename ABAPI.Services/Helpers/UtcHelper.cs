using System.Runtime.CompilerServices;

namespace ABAPI.Services.Helpers;

public static class UtcHelper
{
    public static string ToUtcString(this DateTime dateTime)
    {
        return string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", dateTime.ToUniversalTime());
       // return dateTime.ToUniversalTime()
       //            .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
    }
}