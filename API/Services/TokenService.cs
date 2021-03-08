using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace API.Services
{
    public class TokenService: ITokenService
    {
        private readonly SymmetricSecurityKey _key; // type of encprition
        public TokenService(IConfiguration config )
        {
            // key bytelara çeviriyoruz.
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        
        {
            var claims = new List<Claim>
            {
                // JWT tutacağımız bilgileri geçiyoruz. Payload kısmı- Crediations kısmı.
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            // Tokenin kullanacağı algoritmanın seçimi.
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            
            // Token içereceği tüm bilgiler
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // claims bilgileri
                Expires = DateTime.Now.AddDays(7), // token süresi
                SigningCredentials = creds // sign bilgileri - algoritma ve type
            };
            var tokenHandler = new JwtSecurityTokenHandler(); // token handler ve token yaratmak için
            var token = tokenHandler.CreateToken(tokenDescriptor); // token create ediyoruz.
            return tokenHandler.WriteToken(token); // token dönüyoruz.
        }
        
    }
}