using Microsoft.AspNetCore.Identity;

namespace LocalTrader.Data.Account;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<UserId>
{
    private ApplicationUser(){}
    
    public const int DisplayNameMaxLength = 40;
    public const int DisplayNameMinLength = 5;
    public required string DisplayName { get; set; }

    public static ApplicationUser Create(string displayName)
    {
        return new ApplicationUser()
        {
            Id = UserId.New(),
            DisplayName = displayName
        };
    }
}