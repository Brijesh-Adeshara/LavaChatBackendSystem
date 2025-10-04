using System.Text.Json;
using LavaChatBackend.Authorization;
using LavaChatBackend.Entities;
using LavaChatBackend.Helpers;
using LavaChatBackend.Models.Accounts;
using RestSharp;

namespace LavaChatBackend.Services
{

    public interface IAccountService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    }
    public class AccountService : IAccountService
    {
        private readonly DataContext _context;
        private readonly IJwtUtils _jwtUtils;

        public AccountService(DataContext context, IJwtUtils jwtUtils)
        {
            _context = context;
            _jwtUtils = jwtUtils;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            // Verify access token with Facebook API
            var client = new RestClient("https://graph.facebook.com/v20.0");
            var request = new RestRequest($"me?access_token={model.AccessToken}&fields=id,name,email,birthday,picture.type(large)");
            var response = await client.GetAsync(request);

            if (!response.IsSuccessful)
                throw new AppException("Invalid Facebook access token");

            // Parse Facebook response
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(response.Content!);
            var facebookId = long.Parse(data!["id"].ToString()!);
            var name = data["name"].ToString();
            var email = data.ContainsKey("email") ? data["email"].ToString() : null;
            var birthday = data.ContainsKey("birthday") ? DateTime.Parse(data["birthday"].ToString()!) : (DateTime?)null;
            var picture = data.ContainsKey("picture")
                ? JsonSerializer.Deserialize<JsonElement>(data["picture"].ToString()!).GetProperty("data").GetProperty("url").GetString()
                : null;
            var phone = data.ContainsKey("phone") ? data["phone"].ToString() : null;

            // Find or create account
            var account = _context.Accounts.SingleOrDefault(x => x.FacebookId == facebookId);

            if (account == null)
            {
                // Create new account on first login
                account = new Account
                {
                    FacebookId = facebookId,
                    Name = name,
                    Email = email,
                    CreatedAt = DateTime.UtcNow,
                    Birthday = birthday,
                    ProfilePictureUrl = picture,
                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Update existing account info
                account.Name = name;
                account.Email = email;
                account.Birthday = birthday;
                account.ProfilePictureUrl = picture;
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
            }

            // Generate JWT token
            var token = _jwtUtils.GenerateJwtToken(account);
            return new AuthenticateResponse(account, token);
        }


    }
}
