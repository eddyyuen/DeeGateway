using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DeeGateway.Utils.Jwt
{
    public class JwtHelper
    {
        private string mIssuer = null;

        private string mAudience = null;

        private SecurityKey mSecurityKey;

        private SigningCredentials mSigningCredentials;

        private TokenValidationParameters mTokenValidation = new TokenValidationParameters();

        private JwtSecurityTokenHandler mJwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public JwtHelper() : this(null, null)
        {

        }
        public static JwtHelper Default
        {
            get;
            set;
        }
        public static void Init()
        {
            string key = "12qyg4coej88uqromo0xdmx4y0il5dn5y7b72tlb3imba677ht1p1xlfcnh36mk5u3xzjktfara29axvzk85apfplun7oslbe1m20c148p5d519kja5wvg7lmn5v4a5ou";
            Default = new JwtHelper("DeeGateway", "DeeGateway", key);
        }

        public JwtHelper(string issuer, string audience, string key = "12qyg4coej88uqromo0xdmx4y0il5dn5y7b72tlb3imba677ht1p1xlfcnh36mk5u3xzjktfara29axvzk85apfplun7oslbe1m20c148p5d519kja5wvg7lmn5v4a5ou")
        {
            mIssuer = issuer;
            mAudience = audience;
            mSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            if (string.IsNullOrEmpty(mIssuer))
            {
                mTokenValidation.ValidateIssuer = false;
            }
            else
            {
                mTokenValidation.ValidIssuer = mIssuer;
            }
            if (string.IsNullOrEmpty(mAudience))
            {
                mTokenValidation.ValidateAudience = false;
            }
            else
            {
                mTokenValidation.ValidAudience = mAudience;
            }
            mTokenValidation.IssuerSigningKey = mSecurityKey;
            mSigningCredentials = new SigningCredentials(mSecurityKey, SecurityAlgorithms.HmacSha256);
            Expires = 60 * 24;
        }

        public int Expires { get; set; }

        public string CreateToken(string name, string role, int timeout = 20)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Name", name));
            claimsIdentity.AddClaim(new Claim("Role", role));
            return mJwtSecurityTokenHandler.CreateEncodedJwt(mIssuer, mAudience, claimsIdentity, DateTime.Now.AddMinutes(-5.0), DateTime.Now.AddMinutes(timeout), DateTime.Now, mSigningCredentials);
        }
        public ClaimsPrincipal ValidateToken(string token)
        {
            SecurityToken securityToken;
            ClaimsPrincipal result = null;

            try
            {
                result = mJwtSecurityTokenHandler.ValidateToken(token, mTokenValidation, out securityToken);
            }
            catch(Exception ex) 
            {

            }
            return result;
        }

        public UserInfo GetUserInfo(string token)
        {
            UserInfo userInfo = new UserInfo();
            if (!string.IsNullOrEmpty(token))
            {
                ClaimsIdentity claimsIdentity = ValidateToken(token)?.Identity as ClaimsIdentity;
                userInfo.Name = claimsIdentity?.Claims?.FirstOrDefault((Claim c) => c.Type.ToLower() == "name")?.Value;
                userInfo.Role = claimsIdentity?.Claims?.FirstOrDefault((Claim c) => c.Type.ToLower() == "role")?.Value;
                if (String.IsNullOrEmpty(userInfo.Name))
                {
                    userInfo.Name = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type.ToLower() == "userid")?.Value;
                }
                if (String.IsNullOrEmpty(userInfo.Role))
                {
                    userInfo.Role = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type.ToLower() == "openid")?.Value;
                }
                
            }
            return userInfo;
        }

        public class UserInfo
        {
            public string Name;

            public string Role;

        }
        public void UpdateTokenKey(string key)
        {
            mSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            mTokenValidation.IssuerSigningKey = mSecurityKey;
        }
    }
}
