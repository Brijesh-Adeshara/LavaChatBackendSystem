using System.ComponentModel.DataAnnotations;

namespace LavaChatBackend.Models.Accounts
{
    public class AuthenticateRequest
    {
        [Required]
        public string? AccessToken { get; set; }
    }
}
