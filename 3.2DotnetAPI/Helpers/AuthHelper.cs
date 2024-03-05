using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers {
    public class AuthHelper{
        private IConfiguration _config; 
        public AuthHelper(IConfiguration config){
            _config = config;
        }

        public byte[] GetPasswordHash(string password,byte[] passwordSalt){
            string passwordSaltPlusString = _config.GetSection("AppSettings:Passwordkey").Value + Convert.ToBase64String(passwordSalt);
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100,
                numBytesRequested: 256/8
            );
            return passwordHash;
        }

        public string CreateToken(int userId){
            Claim[] claims = new Claim[]{
                new("userId", userId.ToString()),
                new("testValue", "hola mundo desde claim")
            };

            string? tokenkeyString = _config.GetSection("Appsettings:TokenKey").Value;

            SymmetricSecurityKey tokenkey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    tokenkeyString != null ? tokenkeyString : ""
                )
            ); 

            SigningCredentials credentials = new SigningCredentials(tokenkey,SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor(){
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}