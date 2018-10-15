using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Linq;
namespace MarketingMovie
{
    public class CosmosService
    {
        DocumentClient docClient;

        public async Task<List<Movie>> GetAllMovies()
        {
            List<Movie> allMovies = new List<Movie>();


            var colUrl = UriFactory.CreateDocumentCollectionUri("movies-and-reviews", "movies");

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
            List<Review> theReviews = new List<Review>();

            docClient = new DocumentClient(new Uri("https://marketing-movie.documents.azure.com:443/"),
                                           "jEqsDLbCWpd3INDhH07d82SWDviSWr1O14crrrnTzw6r0wBkW2DrF5cQAaeaucuILD79ggqSE7IhLd7HbgC9Jw==");

            var colUrl = UriFactory.CreateDocumentCollectionUri("movies-and-reviews", "reviews");

            var feedOptions = new FeedOptions() { EnableCrossPartitionQuery = true };

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
