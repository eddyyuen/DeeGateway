using System;
using System.Linq;
using System.Text;
using DeeGateway.Repository.PluginModel;
using SqlSugar;

namespace DeeGateway.Repository.PluginModel
{

    public partial class rewrite_rule_ext : base_rule
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public RewriteHandle handle { get; set; }

       
    }
    public class RewriteHandle
    {
        public string url { get; set; }
    }
}
