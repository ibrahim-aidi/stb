using API.Data;
using API.Dtos;
using API.Models;
using API.Services.EmailSenderService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSender;

        public AuthService(DataContext context,
            IConfiguration configuration, 
            IEmailSenderService emailSender)
        {
            _context = context;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        

        public async Task<ServiceResponse<string>> Login(string Login, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login.Equals(Login));
            if (user == null)
            {
                
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user)
        {
            if (await UserLoginExists(user.Login) || await UserEmailExists(user.Email) )
            {
                return new ServiceResponse<int>
                {
                    Message = "User already exists."
                };
            }
            var password = GenerateRandomPassword();
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var emailUser = new UserForEmail
            {
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = password
            };

            await _emailSender.SendPasswordByEmailAsync(emailUser);

            return new ServiceResponse<int> { Data = user.Id, Message = "Registration successful!" };
        }

        public async Task<bool> UserLoginExists(string Login)
        {
            if (await _context.Users.AnyAsync(user => user.Login
                 .Equals(Login)))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UserEmailExists(string Email)
        {
            if (await _context.Users.AnyAsync(user => user.Email
                 .Equals(Email)))
            {
                return true;
            }
            return false;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash =
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenKey = _configuration["AppSettings:Token"] ?? throw new InvalidOperationException("secret key is not found");
            // Retrieve the token key from appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                   
                    Message = "User not found."
                };
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true, Message = "Password has been changed." };
        }

        public async Task<User?> GetUserByLogin(string Login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login.Equals(Login));
        }

        public string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@";
            var random = new Random();

            // Generate a password with at least 8 characters
            int passwordLength = random.Next(10, 16); // Random length between 10 and 15
            char[] password = new char[passwordLength];

            // Add at least one uppercase letter, one lowercase letter, one digit, and one '@'
            password[0] = validChars[random.Next(validChars.Length)]; 
            password[1] = validChars[random.Next(validChars.Length)]; 
            password[2] = validChars[random.Next(validChars.Length)]; 
            password[3] = '@'; // Symbol

            // Fill the rest of the password with random characters
            for (int i = 4; i < passwordLength; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }

            // Shuffle the password characters randomly
            for (int i = passwordLength - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (password[i], password[j]) = (password[j], password[i]);
            }

            return new string(password);
        }


    }
}
