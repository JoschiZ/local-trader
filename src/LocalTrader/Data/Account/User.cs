using LocalTrader.Api.Account.Collections;
using LocalTrader.Data.Account.Collections;
using LocalTrader.Shared.Api.Account.Users;
using Microsoft.AspNetCore.Identity;

namespace LocalTrader.Data.Account;

// Add profile data for application users by adding properties to the ApplicationUser class
public class User : IdentityUser<UserId>
{
    private User(){}
    
    public const int DisplayNameMaxLength = 40;
    public const int DisplayNameMinLength = 5;
    public required string DisplayName { get; init; }
    public List<CollectionCard> Collection { get; init; } = [];

    public static User Create(string displayName)
    {
        return new User()
        {
            Id = UserId.New(),
            DisplayName = displayName
        };
    }
}