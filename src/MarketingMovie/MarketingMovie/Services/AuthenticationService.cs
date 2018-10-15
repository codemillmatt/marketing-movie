using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace MarketingMovie
{
    public class AuthenticationService
    {
        #region Environment Variables

        static readonly string Tenant = "marketingmovie.onmicrosoft.com";
        static readonly string ClientID = "4f1ef9a7-c510-4154-808c-018022d26b9d";
        static readonly string SignUpAndInPolicy = "B2C_1_Marketing-Video-Sign-In";

        static readonly string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";
        static readonly string Authority = $"{AuthorityBase}{SignUpAndInPolicy}";

        static readonly string[] Scopes = { "https://marketingmovie.onmicrosoft.com/backend/rvw_all" };

        static readonly string RedirectUrl = $"msal{ClientID}://auth";

        #endregion

        readonly PublicClientApplication msaClient;

        public string DisplayName
        {
            get;
            set;
        }


        public AuthenticationService()
        {
            msaClient = new PublicClientApplication(ClientID);//, Authority);
            msaClient.ValidateAuthority = false;

            msaClient.RedirectUri = RedirectUrl;
        }

        UIParent parent;
        public UIParent UIParent { get => parent; set => parent = value; }

        public async Task<AuthenticationResult> Login()
        {
            //TODO: Finish up getting iOS running with newest MSAL

            AuthenticationResult msalResult = null;

            // Running on Android - we need UIParent to be set to the main Activity
            if (UIParent == null && Device.RuntimePlatform == Device.Android)
                return msalResult;

            try
            {
                msalResult = await msaClient.AcquireTokenAsync(Scopes,
                                                           GetUserByPolicy(msaClient.Users,
                                                                           SignUpAndInPolicy),
                                                           UIBehavior.ForceLogin,
                                                           null,
                                                           null,
                                                           Authority,
                                                           UIParent);


                if (msalResult?.User != null)
                {

                    var parsed = ParseIdToken(msalResult.IdToken);
                    DisplayName = parsed["name"]?.ToString();
                }

                return msalResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return msalResult;
        }

        IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);

                if (userIdentifier.EndsWith(policy.ToLower(), StringComparison.OrdinalIgnoreCase)) return user;
            }

            return null;
        }

        string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

        JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }
    }
}
