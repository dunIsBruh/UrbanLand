namespace UrbanLand.Web.Identity.Configurations;

public class IdentityConfiguration
{
    public const string SectionName = "Identity";
    
    public bool RequireConfirmedEmail { get; set; } = false;
    public int PasswordMinLength { get; set; } = 8;
    public bool PasswordRequireDigit { get; set; } = true;
    public bool PasswordRequireNonAlphanumeric { get; set; } = false;
    public int MaxFailedLoginAttempts { get; set; } = 5;
}