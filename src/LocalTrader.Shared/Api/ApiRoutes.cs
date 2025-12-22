namespace LocalTrader.Shared.Api;

public static class ApiRoutes
{
    public static class Account
    {
        public static class Collections
        {
            public static class Magic
            {
                public const string AddCard = "collections/magic/cards";
            }
            public const string DeleteCard = "collections/cards/{cardId}";
            public const string UpdateCard = "collections/cards/{cardId}";
        }
    }
}