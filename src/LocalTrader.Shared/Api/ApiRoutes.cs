using System.Linq.Expressions;
using LocalTrader.Shared.Api.Magic.Collection;
using LocalTrader.Shared.Api.Magic.Wants.Cards;
using LocalTrader.Shared.Api.Magic.Wants.Lists;

namespace LocalTrader.Shared.Api;

public static class ApiRoutes
{
    public static class Magic
    {
        private const string RoutePrefix = "magic/";
        public static class Wants
        {
            public static class Lists
            {
                public const string Create = RoutePrefix + "want-lists";

                public const string Delete = RoutePrefix + "want-lists/{@id}";
                public static readonly Expression<Func<DeleteWantListRequest, object>> DeleteBinding = x => new { x.Id };

                public const string Get = RoutePrefix + "want-lists/{@id}";
                public static readonly Expression<Func<GetWantListRequest, object>> GetBinding = x => new { x.WantListId };

                public const string Update = RoutePrefix + "want-lists/{@id}";
                public static readonly Expression<Func<UpdateWantListRequest, object>> UpdateBinding = x => new { x.Id };
            }
            public static class Cards
            {
                public const string Remove = "want-lists/cards/{@id}";
                public static readonly Expression<Func<RemoveWantedCardRequest, object>> RemoveBinding = x => new { x.WantedMagicCardId };
                public const string Update = "want-lists/cards/{@id}";
                public static readonly Expression<Func<UpdateWantedCardRequest, object>> UpdateBinding = x => new { x.WantedMagicCardId };
                public const string Add = "want-lists/{@id}";
                public static readonly Expression<Func<AddWantedMagicCardRequest, object>> AddBinding = x => new { x.WantListId };
            }
        }
        public static class Collection
        {
            public const string DeleteCard = "collections/cards/{@id}";
            public const string UpdateCard = "collections/cards/{@id}";
            public static readonly Expression<Func<UpdateCardRequest, object>> UpdateBinding = x => new { x.CardId };
            public const string AddCard = "collections/magic/cards";
        }
    }
}