using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using LocalTrader.Shared.Constants;

namespace LocalTrader.Shared.Data.Account;

[ComplexType]
public record ActionRadius
{
    public required Meter Radius { get; set; }
    public required Longitute Longitude { get; init; }
    public required Latitude Latitude { get; init; }
}

public sealed class ActionRadiusValidator : AbstractValidator<ActionRadius>
{
    public ActionRadiusValidator()
    {
        RuleFor(r => r.Radius).InclusiveBetween(Meter.Empty, LocationConstants.MaximumRadius);
        RuleFor(r => r.Latitude).InclusiveBetween(LocationConstants.MinimumLatitude, LocationConstants.MaximumLatitude);
        RuleFor(x => x.Longitude).InclusiveBetween(LocationConstants.MinimumLongitute, LocationConstants.MaximumLongitute);
    }
}