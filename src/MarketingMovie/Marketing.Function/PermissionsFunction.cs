using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Marketing.Function
{
    public static class Function1
    {
        static readonly string publicUserId = System.Environment.GetEnvironmentVariable("PublicUserName");
        static readonly string dbName = Environment.GetEnvironmentVariable("CosmosDBName");
        static readonly string reviewsCollectionName = Environment.GetEnvironmentVariable("ReviewsCollection");
        static readonly string moviesCollectionName = Environment.GetEnvironmentVariable("MoviesCollection");

        [FunctionName("MovieReviewPermission")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
            [DocumentDB(databaseName: "moviereview-db", collectionName: "reviews", ConnectionStringSetting = "CosmosConnectionString")] DocumentClient client,
            TraceWriter log)
        {
            try
            {
                // Figure out if we're logged in or not
                var userId = GetUserId(log);

                log.Info($"User ID: {userId}");

                var token = await GetPartitionPermission(userId, client, dbName, reviewsCollectionName);

                //var serializedToken = SerializePermission(token);

                return req.CreateResponse<string>(HttpStatusCode.OK, token.Token);
            }
            catch (Exception ex)
            {
                log.Error($"***Something went wrong {ex.Message}");

                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [FunctionName("MoviePermission")]
        public static async Task<HttpResponseMessage> GetMovieReadPermission(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
            [DocumentDB(databaseName: "movies-and-reviews", collectionName: "movies", ConnectionStringSetting = "CosmosConnectionString")]DocumentClient client,
            TraceWriter log)
        {
            try
            {
                // Figure out if we're logged in or not
                var userId = publicUserId;

                var token = await GenerateReadPermission(userId, client, dbName, moviesCollectionName);

                //var serializedToken = SerializePermission(token);

                return req.CreateResponse<string>(HttpStatusCode.OK, token.Token);
            }
            catch (Exception ex)
            {
                log.Error($"***Something went wrong {ex.Message}");

                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        static async Task<Permission> GenerateReadPermission(string userId, DocumentClient client, string databaseId, string collectionId)
        {
            Permission readPermission = new Permission();

            string permissionId = "";

            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            Uri userUri = UriFactory.CreateUserUri(databaseId, userId);

            permissionId = $"{userId}-read-{collectionId}";

            try
            {
                // the permission ID's format: {userID}-{documentID}
                Uri permissionUri = UriFactory.CreatePermissionUri(databaseId, userId, permissionId);

                readPermission = await client.ReadPermissionAsync(permissionUri);
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // The permission was not found - either the user (and permission) doesn't exist or permission doesn't exist
                    await CreateUserIfNotExistAsync(userId, client, databaseId);

                    var newPermission = new Permission
                    {
                        PermissionMode = PermissionMode.Read,
                        Id = permissionId,
                        ResourceLink = collectionUri.ToString()
                    };

                    readPermission = await client.CreatePermissionAsync(userUri, newPermission);
                }
                else { throw ex; }
            }

            return readPermission;
        }

        static async Task<Permission> GetPartitionPermission(string userId, DocumentClient client, string databaseId, string collectionId)
        {
            Permission partitionPermission = new Permission();
            bool isLimitedPartition = true;
            string permissionId = "";

            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            Uri userUri = UriFactory.CreateUserUri(databaseId, userId);

            if (userId == publicUserId)
            {
                permissionId = $"{userId}-partition-limited-{collectionId}";
                isLimitedPartition = true;
            }
            else
            {
                permissionId = $"{userId}-partition-all-{collectionId}";
                isLimitedPartition = false;
            }

            try
            {
                // the permission ID's format: {userID}-{documentID}
                Uri permissionUri = UriFactory.CreatePermissionUri(databaseId, userId, permissionId);

                partitionPermission = await client.ReadPermissionAsync(permissionUri);
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // The permission was not found - either the user (and permission) doesn't exist or permission doesn't exist
                    await CreateUserIfNotExistAsync(userId, client, databaseId);

                    var newPermission = new Permission
                    {
                        PermissionMode = PermissionMode.Read,
                        Id = permissionId,
                        ResourceLink = collectionUri.ToString()
                    };

                    if (isLimitedPartition)
                        newPermission.ResourcePartitionKey = new PartitionKey(false);

                    partitionPermission = await client.CreatePermissionAsync(userUri, newPermission);
                }
                else { throw ex; }

            }

            return partitionPermission;
        }

        static async Task CreateUserIfNotExistAsync(string userId, DocumentClient client, string databaseId)
        {
            try
            {
                await client.ReadUserAsync(UriFactory.CreateUserUri(databaseId, userId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateUserAsync(UriFactory.CreateDatabaseUri(databaseId), new User { Id = userId });
                }
            }

        }

        static string SerializePermission(Permission permission)
        {
            string serializedPermission = "";

            using (var memStream = new MemoryStream())
            {
                permission.SaveTo(memStream);
                memStream.Position = 0;

                using (StreamReader sr = new StreamReader(memStream))
                {
                    serializedPermission = sr.ReadToEnd();
                }
            }

            return serializedPermission;
        }

        static string GetUserId(TraceWriter log)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var cp = (ClaimsPrincipal)Thread.CurrentPrincipal;
                log.Info($"Thread.CurrentPrincipal Name: {Thread.CurrentPrincipal.Identity.Name}");

                return publicUserId;
            }
            else
                return "codemillmatt";

            //var claimsPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;

            //var objectClaimTypeName = @"http://schemas.microsoft.com/identity/claims/objectidentifier";

            //var objectClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == objectClaimTypeName);

            //if (objectClaim == null)
            //    return publicUserId;
            //else
            //    return objectClaim.Value;
        }


    }
}
