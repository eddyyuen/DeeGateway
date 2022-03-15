using BeetleX.FastHttpApi;
using DeeGateway.Configuration.Filter;
using DeeGateway.Utils.Jwt;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Configuration.Controller
{
    [Options(AllowOrigin = "*",AllowHeaders ="*")]
    [Controller(BaseUrl = "__manage/user/")]
    [TokenFilter]
    
    public class User
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [SkipFilter(typeof(TokenFilter))]
        [Post]
       public JsonResult Login(string username ,string password, IHttpContext context)
        {
            var retCode = 1;
            var token = "";

            if (Config.Default.UserName == username && Config.Default.Password == password)
            {
                retCode = 0;
                token = "Bearer " + JwtHelper.Default.CreateToken(username, "admin", 1500);
            }
            var ret = new
            {
                retCode,
                token
            };
           
            return new JsonResult(ret);
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        [Post]
        public JsonResult ChangePwd(string oldPassword, string password)
        {
            var retCode = 1;
            if (!string.IsNullOrEmpty(password) && Config.Default.Password == oldPassword)
            {
                retCode = 0;
                Config.Default.Password = password;
                Config.Default.Save();

            }
            var ret = new
            {
                retCode,
                message=""
            };
            return new JsonResult(ret);
        }
    }
}
