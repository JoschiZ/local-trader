using LocalTrader.Shared.Api.Account.Users;
using LocalTrader.Shared.Api.Account.Wants.Cards;

namespace LocalTrader.Shared.Api.Account.Wants.WantLists;

public record WantListDto(
    string Name,
    UserId Owner,
    Accessibility Accessibility,
    WantedCardDto[] WantedCards
    );