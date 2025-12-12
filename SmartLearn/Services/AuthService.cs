using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartLearn.Data;
using SmartLearn.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartLearn.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string?> GenerateJwtToken(User user)
        {
            var jwtSettings=_configuration.GetSection("Jwt");
            var key= Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            var issuer=jwtSettings.GetValue<string>("Issuer");
            var audience=jwtSettings.GetValue<string>("Audience");

            var claims =new  List<Claim>{ 
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor=new  SecurityTokenDescriptor
            {
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.UtcNow.AddDays(2),
                Issuer=issuer,
                Audience=audience,
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler=new JwtSecurityTokenHandler();
            var token=tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var existingUser=await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser ==null)
            {
                return "User not found.";
            }
            bool isPasswordValid=BCrypt.Net.BCrypt.Verify(password, existingUser.PasswordHash);
            if (!isPasswordValid)
            {
                return "Invalid password.";
            }

            return GenerateJwtToken(existingUser).Result;
        }

        public async Task<string?> RegisterAsync(User obj, string password)
        {
            // check if user with the same email already exists
            var existingUser=await _context.Users.FirstOrDefaultAsync(u => u.Email == obj.Email);
            if (existingUser !=null)
            {
                return "User with the same email exists."; 
            }
            // if not, create new user
            obj.PasswordHash= BCrypt.Net.BCrypt.HashPassword(password);
            await _context.Users.AddAsync(obj);
            await _context.SaveChangesAsync();
            return "User registered successfully.";
        }
    }
}
