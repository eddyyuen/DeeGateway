using BeetleX.FastHttpApi;

namespace DeeGateway.Configuration.Filter
{
    public class AdminFilter : TokenFilter
    {
        public override bool Executing(ActionContext context)
        {
            if (base.Executing(context))
            {
                if (context.HttpContext.Data["_userRole"] != "admin")
                {
                    context.Result = new JsonResult(new { retCode = 401, message = "Unauthorized" });
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
