using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeeGateway.Repository.PluginModel;
using SqlSugar;

namespace DeeGateway.Repository.PluginModel
{

    public partial class header_rule_ext : base_rule
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public List<HeaderHandle> handle { get; set; }

       
    }
    public class HeaderHandle
    {
        public string key { get; set; }
        public string value { get; set; }
        public string default_value { get; set; }
    }
}
