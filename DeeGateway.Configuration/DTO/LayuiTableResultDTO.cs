using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Configuration.DTO
{
    public class LayuiTableResultDTO
    {
        public int code { get; set; } = 0;
        public string msg { get; set; } = string.Empty;
        public int count { get; set; } = 0;
        public object data { get; set; } 
    }
    public class ReturnResultDTO
    {
        /// <summary>
        /// 0-成功 1-失败
        /// </summary>
        public int code { get; set; } = 0;
        public string msg { get; set; } = string.Empty;

    }
}
