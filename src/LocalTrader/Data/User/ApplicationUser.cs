using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StronglyTypedIds;

namespace LocalTrader.Data.User;

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

internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.DisplayName).HasMaxLength(ApplicationUser.DisplayNameMaxLength).IsRequired();
    }
}

[StronglyTypedId("guid-v7")]
public readonly partial struct UserId;


public class AppClaimsPrinciple : ClaimsPrincipal
{
    public AppClaimsPrinciple(ClaimsPrincipal principal) : base(principal)
    {
    }

    public UserId UserId
    {
        get
        {
            var idValue = FindFirst(ClaimTypes.Sid)?.Value;
            return idValue is null ? UserId.Empty : UserId.Parse(idValue);
        }
    }
}