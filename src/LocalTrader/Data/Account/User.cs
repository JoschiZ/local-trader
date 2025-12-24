using LocalTrader.Data.Account.Wants;
using LocalTrader.Data.Magic;
using LocalTrader.Shared.Api.Account.Users;
using LocalTrader.Shared.Data.Account;
using Microsoft.AspNetCore.Identity;

namespace LocalTrader.Data.Account;

// Add profile data for application users by adding properties to the ApplicationUser class
public class User : IdentityUser<UserId>
{
    private User(){}
    
    public const int DisplayNameMaxLength = 40;
    public const int DisplayNameMinLength = 5;
    public required string DisplayName { get; init; }
    
    public List<MagicWantList> MagicWantLists { get; init; } = [];
    public List<CollectionMagicCard> MagicCollection { get; set; } = [];

    public static User Create(string displayName)
    {
        return new User
        {
            Id = UserId.New(),
            DisplayName = displayName
        };
    }
}