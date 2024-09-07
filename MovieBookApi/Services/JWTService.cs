using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieBookApi.Controllers;
using MovieBookApi.Models.Db;
using MovieBookApi.Models.ResultClasses;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieBookApi.Services
{
    public interface IJWTService
    {
        public string GetUserId(string email, string password);
        public string GetUserName(string email, string question,string answer);
        public string GetUserName(string email,string password);
        public bool AleadyUser(string email);
        public string RegisterUser(iUser user);
        public string? AuthenticateForgotPassword(string email,string question,string answer);
        public string? AuthenticateUser(string email, string password);
        public string? GenerateToken(string role,string uid);
        public bool VerifyToken(string token);
        public string GetUserId(string email, string sercurityQuestion, string password);
    }
    public class JWTService : IJWTService
    {
        public readonly MovieBookDbContext context;
        public readonly IConfiguration Configuration;
        private readonly ILogger<JWTService> _logger;

        public JWTService(MovieBookDbContext context, IConfiguration configuration, ILogger<JWTService> _logger)
        {
            this.context = context;
            this.Configuration = configuration;
            this._logger = _logger;
        }

        public string GetUserId(string email, string password)
        {
            var res = from e in context.Users where e.Email == email && e.PasswordHash == password select e.UserId;
            return res.FirstOrDefault()!;
        }
        public bool AleadyUser(string email)
        {
            return context.Users.Any(u => u.Email == email);
        }
        public string? AuthenticateUser(string email, string password)
        {
            var res = from e in context.Users where e.Email == email && e.PasswordHash == password select e.Role;

            return res.FirstOrDefault();
        }

        public string? GenerateToken(string role, string uid)
        {
            string audience, issuer, secret;
            audience = Configuration.GetValue<string>("Audience")!;
            issuer = Configuration.GetValue<string>("Issuer")!;
            secret = Configuration.GetValue<string>("Secret")!;

            byte[] secretbytes = System.Text.Encoding.UTF8.GetBytes(secret);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,uid),
                new Claim(JwtRegisteredClaimNames.Iss,issuer),
                new Claim(JwtRegisteredClaimNames.Aud,audience),
                new Claim(ClaimTypes.Role,role),
            };
            var key = new SymmetricSecurityKey(secretbytes);
            var signingCreddentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var Securitytoken = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.Now.AddDays(30), signingCredentials: signingCreddentials);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(Securitytoken);
            return token;
        }

        public string RegisterUser(iUser user)
        {
            var newUserIDParam = new SqlParameter
            {
                ParameterName = "@NewUserID",
                SqlDbType = SqlDbType.VarChar,
                Size = 36,
                Direction = ParameterDirection.Output
            };

            context.Database.ExecuteSqlRaw(
                "EXEC InsertUser @FullName, @PasswordHash, @Email, @PhoneNumber, @SecurityQuestion, @SecurityAnswer, @NewUserID OUT",
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PhoneNumber", user.PhoneNumber),
                new SqlParameter("@SecurityQuestion", user.SecurityQuestion),
                new SqlParameter("@SecurityAnswer", user.SecurityAnswer),
                newUserIDParam
            );

            return newUserIDParam.SqlValue.ToString();
        }

        public bool VerifyToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(token))
            {
                throw new ArgumentException("Invalid JWT token format.");
            }

            var jwtToken = jwtHandler.ReadJwtToken(token);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");

            if (expClaim == null)
            {
                throw new ArgumentException("The token does not contain an 'exp' claim.");
            }

            var expirationTimeUnix = long.Parse(expClaim.Value);
            var expirationTimeUtc = DateTimeOffset.FromUnixTimeSeconds(expirationTimeUnix).UtcDateTime;
            //_logger.LogInformation(expirationTimeUtc.ToString());
            //_logger.LogInformation(DateTime.UtcNow.ToString());
            // Compare expiration time with the current UTC time
            return DateTime.UtcNow < expirationTimeUtc;
        }

        public string? AuthenticateForgotPassword(string email, string question, string answer)
        {
            var res = from e in context.Users where e.Email == email && e.SecurityQuestion == question && e.SecurityAnswer==answer select e.Role;

            return res.FirstOrDefault();
        }

        public string GetUserId(string email, string sercurityQuestion, string answer)
        {
            var res = from e in context.Users where e.Email == email && e.SecurityQuestion == sercurityQuestion && e.SecurityAnswer == answer select e.UserId;

            return res.FirstOrDefault();
        }

        public string GetUserName(string email, string question,string answer)
        {
            var res = from u in context.Users where u.Email == email && u.SecurityQuestion == question && u.SecurityAnswer==answer select u.FullName;
            return res.FirstOrDefault();
        }

        public string GetUserName(string email, string password)
        {
            var res = from u in context.Users where u.Email == email && u.PasswordHash == password select u.FullName;
            return res.FirstOrDefault();
        }
    }
}
