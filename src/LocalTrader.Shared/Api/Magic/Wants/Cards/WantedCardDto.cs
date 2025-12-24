using System.Text.Json.Serialization;
using LocalTrader.Shared.Data.Magic.Cards;
using LocalTrader.Shared.Data.Shared;

namespace LocalTrader.Shared.Api.Magic.Wants.Cards;

[JsonPolymorphic]
[JsonDerivedType(typeof(WantedMagicCardDto))]
public abstract record WantedCardDto(
    string Name,
    WantedMagicCardId Id,
    CardCondition MinimumCondition
    );


public sealed record WantedMagicCardDto(
    string Name,
    WantedMagicCardId Id,
    CardCondition MinimumCondition,
    ScryfallId ScryfallId,
    Uri ScryfallLink) 
    : WantedCardDto(Name, Id, MinimumCondition);