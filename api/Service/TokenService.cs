using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config; 
        private readonly SymmetricSecurityKey _key; 
        private readonly UserManager<AppUser> _userManager; 
        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _config = config; 
            _userManager = userManager; 

            var signingKey = _config["JWT:SigningKey"]
                ?? throw new InvalidOperationException("JWT:SigningKey is not configured");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)); 
        }
        public async Task<string> CreateToken(AppUser user)
        {
            // das sind jetzt die Informationen die in dem JWT gespeichert werden 
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id), // für GetUserAsync
                new Claim(ClaimTypes.Name, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName), "user.Username must not be null")), 
                new Claim(ClaimTypes.Email, user.Email ?? throw new ArgumentNullException(nameof(user.Email), "user.Email must not be null.")),
                // hier müsste die Rolle noch eingetragen werden 
                //new Claim("role", string.Join(",", userRoles)) // Falls du Identity-Rollen nutzt
            }; 
            var roles = await _userManager.GetRolesAsync(user); 

            foreach (var role in roles){
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Nur org_id hinzufügen, wenn der User kein Admin ist
            if (!roles.Contains("Admin"))
            {
                claims.Add(new Claim("org_id", user.CompanyId.ToString() ?? "unknown"));
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); 
            
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // Token läuft nach 2 Stunden ab
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            }; 

            var tokenHandler = new JwtSecurityTokenHandler(); 

            var token = tokenHandler.CreateToken(tokenDescriptor); 

            return tokenHandler.WriteToken(token); 
        }
    }
}

// alte CreateToken Funktion bevor ich den Service nochmal selber zusammenbaue

// public string CreateToken(AppUser user)
//         {
//             var claims = new List<Claim>{
//                 new Claim(JwtRegisteredClaimNames.Email, user.Email),
//                 new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
//                 new Claim(ClaimTypes.NameIdentifier, user.Id), // 🛠 WICHTIG für `User.Identity.Name`
//                 new Claim(ClaimTypes.Name, user.UserName) // 🛠 WICHTIG für `User.Identity.Name`
//                 // new Claim(JwtRegisteredClaimNames.Email, user.Email),
//                 // new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
//             }; 