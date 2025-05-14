using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FlipCardApp.Application.Interfaces;
using FlipCardApp.Domain;
using FlipCardApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FlipCardApp.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Token)> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            
            if (user == null)
                return (false, "User not found");

            if (!VerifyPasswordHash(password, user.PasswordHash))
                return (false, "Wrong password");

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return (true, CreateToken(user));
        }

        public async Task<(bool Success, string Message)> Register(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == user.Username.ToLower()))
                return (false, "Username is already taken");

            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower()))
                return (false, "Email is already registered");

            user.PasswordHash = CreatePasswordHash(password);
            user.Created = DateTime.UtcNow;
            user.LastLogin = DateTime.UtcNow;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return (true, "Registration successful");
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // In a real application, use a proper password hashing algorithm like BCrypt
            // This is a simplified example
            return storedHash == CreatePasswordHash(password);
        }

        private string CreatePasswordHash(string password)
        {
            // In a real application, use a secure password hashing algorithm like BCrypt
            // This is a simplified example using SHA256
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value ?? "default_secure_key_for_development"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
