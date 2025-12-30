using System.Collections.Generic;
using LocalTrader.Data.Magic;

using LocalTrader.Shared.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Account;

// Add profile data for application users by adding properties to the ApplicationUser class
public class User : IdentityUser<UserId>
{
    private User(){}
    
    public const int DisplayNameMaxLength = 40;
    public const int DisplayNameMinLength = 5;
    public required string DisplayName { get; init; }
    
    /// <summary>
    /// The location of a user
    /// </summary>
    public ActionRadius? Location { get; set; }
    /// <summary>
    /// How far the user is willing to go for a trade
    /// </summary>
    public Kilometers? ActionRadius { get; set; }
    
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

internal sealed class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
    }
}