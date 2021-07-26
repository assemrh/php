using iletisim.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iletisim
{
    public class TokenMangement
    {
    }

    internal class TokenManger
    {
        private IConfiguration _config;

        public TokenManger(IConfiguration config)
        {
            _config = config;
        }

        internal UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            if (login.UserName == "Jignesh")
            {
                user = new UserModel { UserName = "Jignesh Trivedi", Email = "test.btest@gmail.com" };
            }
            return user;
        }

        internal string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes( _config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //var roles = new[] { "Admin", "SuperUser" };
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                    new Claim("Id" , userInfo.Id),
                    new Claim("DateOfJoing", DateTime.UtcNow.ToString("yyyy-MM-dd")),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim( "roles",  "Admin"),
                    new Claim( "roles",  "SuperUser")
                };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
