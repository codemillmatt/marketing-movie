using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Linq;
using Microsoft.Azure.Documents;
using Xamarin.Forms;
namespace MarketingMovie
{
    public class CosmosService
    {
        public async Task<List<Movie>> GetAllMovies()
        {
            var functionsService = new FunctionsService();
            var cosmosToken = await functionsService.GetCosmosPermissionToken("", true);

            DocumentClient docClient;
            List<Movie> allMovies = new List<Movie>();

            docClient = new DocumentClient(new Uri(APIKeys.CosmosUrl), cosmosToken);

            var colUrl = UriFactory.CreateDocumentCollectionUri(APIKeys.MovieReviewDB, APIKeys.MovieCollection);

            var docQuery = docClient.CreateDocumentQuery<Movie>(colUrl).AsDocumentQuery();

            while (docQuery.HasMoreResults)
            {
                var queryResults = await docQuery.ExecuteNextAsync<Movie>();

                allMovies.AddRange(queryResults);
            }

            return allMovies;
        }

        public async Task<List<Review>> GetReviewsForMovie(string movieId)
        {
            var idService = DependencyService.Get<IAuthService>();
            var fs = new FunctionsService();
            var cosmosToken = await fs.GetCosmosPermissionToken(idService?.AuthResult?.IdToken, false);
            bool authenticated = false;

            if (!string.IsNullOrEmpty(idService?.AuthResult?.IdToken))
                authenticated = true;

            DocumentClient docClient;
            List<Review> theReviews = new List<Review>();

            docClient = new DocumentClient(new Uri(APIKeys.CosmosUrl), cosmosToken);

            var feedOptions = new FeedOptions() { EnableCrossPartitionQuery = true };

            if (!authenticated)
                feedOptions.PartitionKey = new PartitionKey(false);

            var colUrl = UriFactory.CreateDocumentCollectionUri(APIKeys.MovieReviewDB, APIKeys.ReviewCollection);

            var docQuery = docClient.CreateDocumentQuery<Review>(colUrl, feedOptions)
                                    .Where(r => r.movieId == movieId)
                                    .AsDocumentQuery();

            while (docQuery.HasMoreResults)
            {
                var queryResults = await docQuery.ExecuteNextAsync<Review>();

                theReviews.AddRange(queryResults);
            }

            return theReviews;
        }
    }
}
