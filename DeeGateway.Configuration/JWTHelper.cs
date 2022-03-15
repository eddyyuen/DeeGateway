using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DeeGateway.Configuration
{
    public class JWTHelper2
    {
        public class UserInfo
        {
            public string Name;

            public string Role;
        }

        public const string TOKEN_KEY = "Token";

        private string mIssuer;

        private string mAudience;

        private SecurityKey mSecurityKey;

        private SigningCredentials mSigningCredentials;

        private TokenValidationParameters mTokenValidation = new TokenValidationParameters();

        private JwtSecurityTokenHandler mJwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public int Expires
        {
            get;
            set;
        }

        public static JWTHelper2 Default
        {
            get;
            set;
        }

        public JWTHelper2(string issuer, string audience, byte[] key)
        {
            mIssuer = issuer;
            mAudience = audience;
            mSecurityKey = new SymmetricSecurityKey(key);
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
            mSigningCredentials = new SigningCredentials(mSecurityKey, "HS256");
            Expires = 1440;
        }

        public void ClearToken(HttpResponse response)
        {
            response.SetCookie("Token", "", "/", DateTime.Now);
        }

        public void CreateToken(HttpResponse response, string name, string role, int timeout = 20)
        {
            string value = CreateToken(name, role, timeout);
            response.SetCookie("Token", value, "/", DateTime.Now.AddDays(100.0));
        }

        public string CreateToken(string name, string role, int timeout = 20)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Name", name));
            claimsIdentity.AddClaim(new Claim("Role", role));
            return mJwtSecurityTokenHandler.CreateEncodedJwt(mIssuer, mAudience, claimsIdentity, DateTime.Now.AddMinutes(-5.0), DateTime.Now.AddMinutes(timeout), DateTime.Now, mSigningCredentials);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            SecurityToken validatedToken;
            try
            {
                return  mJwtSecurityTokenHandler.ValidateToken(token, mTokenValidation, out validatedToken);
            }
            catch 
            {
                return null;
            }
           
        }

        public UserInfo GetUserInfo(HttpRequest request)
        {
            string text = request.Cookies["Token"];
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            try
            {
                return GetUserInfo(text);
            }
            catch (Exception ex)
            {
                HttpApiServer server = request.Server;
                if (server.EnableLog(LogType.Info))
                {
                    server.Log(LogType.Info, request.Session, request.RemoteIPAddress + " get token error " + ex.Message);
                }
                return null;
            }
        }

        public UserInfo GetUserInfo(string token)
        {
            UserInfo userInfo = new UserInfo();
            if (!string.IsNullOrEmpty(token))
            {
                ClaimsIdentity claimsIdentity = ValidateToken(token)?.Identity as ClaimsIdentity;
                userInfo.Name = claimsIdentity?.Claims?.FirstOrDefault((Claim c) => c.Type == "Name")?.Value;
                userInfo.Role = claimsIdentity?.Claims?.FirstOrDefault((Claim c) => c.Type == "Role")?.Value;
            }
            return userInfo;
        }

        public static void Init()
        {
            string key = "12qyg4coej88uqromo0xdmx4y0il5dn5y7b72tlb3imba677ht1p1xlfcnh36mk5u3xzjktfara29axvzk85apfplun7oslbe1m20c148p5d519kja5wvg7lmn5v4a5ou";
            Default = new JWTHelper("DeeGateway", "DeeGateway", Encoding.UTF8.GetBytes(key));
        }
    }
}
