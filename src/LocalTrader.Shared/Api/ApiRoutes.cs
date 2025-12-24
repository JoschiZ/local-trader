namespace LocalTrader.Shared.Api;

public static class ApiRoutes
{
    public static class Account
    {
        public static class Collections
        {
            public const string DeleteCard = "collections/cards/{cardId}";
            public const string UpdateCard = "collections/cards/{cardId}";

            public static class Magic
            {
                public const string AddCard = "collections/magic/cards";
            }
        }

        public static class WantLists
        {
            public const string Create = "want-lists";
            public const string Delete = "want-lists/{wantListId}";
            public const string Get = "want-lists/{wantListId}";
            public const string Update = "want-lists/{wantListId}";
            
            public static class Cards
            {
                public const string Remove = "want-lists/cards/{wantedCardId}";
                public const string UpdateCard = "want-lists/cards/{wantedCardId}";
                public static class Magic
                {
                    public const string Add = "want-lists/{wantListId}/magic";
                }
            }

            
        }
    }
}