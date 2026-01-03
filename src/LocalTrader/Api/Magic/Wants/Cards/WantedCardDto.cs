using System;
using LocalTrader.Data;
using LocalTrader.Data.Magic;

namespace LocalTrader.Api.Magic.Wants.Cards;



public sealed record WantedMagicCardDto(
    string Name,
    WantedMagicCardId Id,
    CardCondition MinimumCondition,
    MagicCardId CardId,
    ScryfallId ScryfallId,
    Uri ScryfallLink);