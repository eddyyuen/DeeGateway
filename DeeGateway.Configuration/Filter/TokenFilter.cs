using BeetleX.FastHttpApi;
using DeeGateway.Utils.Jwt;
using DeeGateway.Utils.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Configuration.Filter
{
    public class TokenFilter : FilterAttribute
    {
        public override bool Executing(ActionContext context)
        {
            if (context.HttpContext.Request.Method.ToUpper() == "OPTIONS")
            {
                context.Result = new OptionsResult();               
                return false;
            }

            string token = context.HttpContext.Request.Header[HeaderTypeFactory.AUTHORIZATION];
            
            if(string.IsNullOrWhiteSpace(token) || !token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new JsonResult(new { retCode = 401, message = "Unauthorized" });
                return false;
            }
            else
            {
                token = token.Remove(0, 7);
            }

            JwtHelper.UserInfo userInfo = JwtHelper.Default.GetUserInfo(token);
            if (string.IsNullOrWhiteSpace(userInfo.Name))
            {
                context.Result = new JsonResult(new { retCode = 401, message = "Unauthorized" });
                return false;
            }
            else
            {
                context.HttpContext.Data.SetValue("_userName", userInfo.Name);
                context.HttpContext.Data.SetValue("_userRole", userInfo.Role);
                return true;
            }

            //JWTHelper.UserInfo userInfo = JWTHelper.Default.GetUserInfo(context.HttpContext.Request);
            //if (userInfo == null)
            //{
            //    ActionResult actionResult = new ActionResult(403, "Access to this resource on the server is denied!");
            //    actionResult.Data = "/__admin/login.html";
            //    context.Result = actionResult;
            //    return false;
            //}
            //context.HttpContext.Data.SetValue("_userName", userInfo.Name);
            //context.HttpContext.Data.SetValue("_userRole", userInfo.Role);
            //return true;
        }
    }
}
