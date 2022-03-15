using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeeGateway.Repository.PluginModel;
using SqlSugar;

namespace DeeGateway.Repository.PluginModel
{

    public partial class url_ratelimit_rule_ext : base_rule
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public List<UrlRateLimitHandle> handle { get; set; }

       
    }
    public class UrlRateLimitHandle
    {
        /// <summary>
        /// 限制的次数
        /// </summary>
        public int limit { get; set; }
        /// <summary>
        /// 时间范围
        /// </summary>
        public int second { get; set; }
        /// <summary>
        /// 禁止访问的时间
        /// </summary>
        public int blocktimes { get; set; }
    }
}
