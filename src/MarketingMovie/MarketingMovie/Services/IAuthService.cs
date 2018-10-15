using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace MarketingMovie
{
    public interface IAuthService
    {
        string DisplayName { get; set; }
        AuthenticationResult AuthResult { get; set; }

        Task<AuthenticationResult> Login();

        void Logout();
        UIParent UIParent { get; set; }
    }
}
