namespace UrbanLand.Web.Identity.Infrastructure.Options;

public class IdentityConfiguration
{
    public const string SectionName = "Identity";
    
    public bool RequireConfirmedEmail { get; set; }
    public int PasswordMinLength { get; set; } = 8;
    public bool PasswordRequireDigit { get; set; } = true;
    public bool PasswordRequireNonAlphanumeric { get; set; }
    public int MaxFailedLoginAttempts { get; set; } = 5;
}