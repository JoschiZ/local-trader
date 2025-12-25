using LocalTrader.Shared.Api.Magic.Wants.Cards;
using LocalTrader.Shared.Data.Account;

namespace LocalTrader.Shared.Api.Magic.Wants.Lists;

public record WantListDto(
    string Name,
    UserId Owner,
    Accessibility Accessibility,
    WantedMagicCardDto[] WantedCards
    );