using BeetleX.Buffers;
using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Text;


namespace DeeGateway.Utils.Result
{
    public class OptionsResult : ResultBase
    {
        public OptionsResult()
        {
        }
        public override IHeaderItem ContentType => ContentTypes.JSON;
        public override void Setting(HttpResponse response)
        {
            response.Code = "204";
            response.CodeMsg = "No Content";
            response.Header.Add("Access-Control-Allow-Credentials", "true");
            response.Header.Add("Access-Control-Allow-Headers", "authorization,Content-Type");
            response.Header.Add("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
            response.Header.Add("Access-Control-Allow-Origin", "*");
            response.Header.Add("Access-Control-Max-Age", "86400");
            response.Header.Add("Connection", "Keep-Alive");
            response.Request.ClearStream();
        }

    }
}
