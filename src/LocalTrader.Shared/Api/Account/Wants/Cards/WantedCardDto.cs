using System.Text.Json.Serialization;
using LocalTrader.Shared.Api.Account.Collections;
using LocalTrader.Shared.Data.Cards.Magic;

namespace LocalTrader.Shared.Api.Account.Wants.Cards;

[JsonPolymorphic]
[JsonDerivedType(typeof(WantedMagicCardDto))]
public abstract record WantedCardDto(
    string Name,
    WantedCardId Id,
    CardCondition MinimumCondition
    );


public sealed record WantedMagicCardDto(
    string Name,
    WantedCardId Id,
    CardCondition MinimumCondition,
    ScryfallId ScryfallId,
    Uri ScryfallLink) 
    : WantedCardDto(Name, Id, MinimumCondition);