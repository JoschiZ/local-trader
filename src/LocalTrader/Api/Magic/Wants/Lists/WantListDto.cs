using LocalTrader.Api.Magic.Wants.Cards;
using LocalTrader.Data.Account;

namespace LocalTrader.Api.Magic.Wants.Lists;

public record WantListDto(
    string Name,
    UserId Owner,
    WantListAccessibility Accessibility,
    WantedMagicCardDto[] WantedCards
    );