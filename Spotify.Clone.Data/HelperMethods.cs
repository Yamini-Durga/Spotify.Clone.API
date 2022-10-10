using Microsoft.IdentityModel.Tokens;
using Spotify.Clone.Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Spotify.Clone.Data
{
    public static class HelperMethods
    {
        public static bool CompareEmails(this string existingEmail, string email)
        {
            string[] existingEmailParts = existingEmail.Split('@');
            string[] domainParts = existingEmailParts[1].Split('.');
            string[] emailParts = email.Split('@');
            string[] curDomainParts = emailParts[1].Split('.');

            bool isEmailsEqual = Equals(existingEmailParts[0].ToLower(), emailParts[0].ToLower()) &&
                Equals(domainParts[0].ToLower(), curDomainParts[0].ToLower()) &&
                Equals(domainParts[1].ToLower(), curDomainParts[1].ToLower());

            return isEmailsEqual;
        }
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        public static string CreateToken(User user, string secretKey)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            };
            if(user.Role == SpotifyConstants.Role)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
        public static string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
