using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Services.Models
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public DateTime expires_in { get; set; }
        public string role { get; set; }
        public string fullName { get; set; }

        public AccessToken(JwtSecurityToken securityToken, string role, string fullName)
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token_type = "Bearer";
            expires_in = securityToken.ValidTo;
            this.role = role;
            this.fullName = fullName;
        }
    }
}
