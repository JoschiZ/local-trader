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
                public const string Get = RoutePrefix + "want-lists/{@id}";
                public const string Update = RoutePrefix + "want-lists/{@id}";
            }
            public static class Cards
            {
                public const string Remove = RoutePrefix + "want-lists/cards/{@id}";
                public const string Update = RoutePrefix +"want-lists/cards/{@id}";
                public const string Add = RoutePrefix +"want-lists/{@id}";
                public const string SearchForAvailableCards = RoutePrefix + "want-lists/cards/{@id}/search-available";
            }
        }
        public static class Collection
        {
            public const string DeleteCard = RoutePrefix + "collections/cards/{@id}";
            public const string UpdateCard = RoutePrefix + "collections/cards/{@id}";
            public const string AddCard = RoutePrefix + "collections/magic/cards";
        }
    }

    public static class Account
    {
        public const string ExternalLogin = "Account/PerformExternalLogin";
        public const string Logout = "Account/Logout";
        public const string GetPasskeyCreationOptions = "Account/PasskeyCreationOptions";
        public const string PasskeyRequestOptions = "Account/PasskeyRequestOptions";
        public const string Login = "Account/login";
        public const string SetLocation = "Account/location";

        public static class Manage
        {
            public const string DownloadPersonalData = "Account/Manage/DownloadPersonalData";
            public const string LinkExternalLogin = "Account/Manage/LinkExternalLogin";
        }
    }
}