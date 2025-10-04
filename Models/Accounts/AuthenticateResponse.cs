using System.Security.Principal;
using LavaChatBackend.Entities;

namespace LavaChatBackend.Models.Accounts
{
    public class AuthenticateResponse
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string Token { get; set; }

        public DateTime? Birthday { get; set; }

        public string? ProfilePicture { get; set; }

        public AuthenticateResponse(Account account, string token)
        {
            Name = account.Name;
            Email = account.Email;
            Token = token;
            Birthday = account.Birthday;
            ProfilePicture = account.ProfilePictureUrl;


        }
    }
}
