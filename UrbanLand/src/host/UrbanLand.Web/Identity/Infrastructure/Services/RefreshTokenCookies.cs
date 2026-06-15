namespace UrbanLand.Web.Identity.Infrastructure.Services;

public static class RefreshTokenCookies
{
    private const string CookieName = "refresh_token";
    private const string CookiePath = "/api/auth";

    public static void SetRefreshTokenCookie(this HttpResponse response, string refreshToken, DateTime expiresAt)
    {
        response.Cookies.Append(CookieName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiresAt,
            Path = CookiePath
        });
    }

    public static void ClearRefreshTokenCookie(this HttpResponse response)
    {
        response.Cookies.Delete(CookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = CookiePath
        });
    }

    public static string? GetRefreshTokenCookie(this HttpRequest request)
    {
        return request.Cookies[CookieName];
    }
}
