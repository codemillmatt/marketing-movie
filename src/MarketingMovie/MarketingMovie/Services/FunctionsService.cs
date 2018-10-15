using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MarketingMovie
{
    public class FunctionsService
    {
        public async Task<string> GetCosmosPermissionToken(string accessToken, bool isForMovieOnly)
        {
            var baseUri = new Uri(APIKeys.BrokerUrlBase);
            var client = new HttpClient { BaseAddress = baseUri };
            Uri brokerUrl;

            if (isForMovieOnly)
                brokerUrl = new Uri(baseUri, APIKeys.ReadMoviePath);
            else
                brokerUrl = new Uri(baseUri, APIKeys.ReviewPermissionPath);

            var request = new HttpRequestMessage(HttpMethod.Get, brokerUrl);

            // Here check if there's a token or not
            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var token = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

            return token;


        }
    }
}
