using System;
namespace MarketingMovie
{
    public class APIKeys
    {
        public static string CosmosUrl = "https://marketing-movie.documents.azure.com:443/";

        public static string MovieReviewDB = "movies-and-reviews";
        public static string ReviewCollection = "reviews";
        public static string MovieCollection = "movies";

        public static string BrokerUrlBase = "https://marketing-movie.azurewebsites.net";
        public static string ReviewPermissionPath = "api/MovieReviewPermission";
        public static string ReadMoviePath = "api/MoviePermission";
    }
}
