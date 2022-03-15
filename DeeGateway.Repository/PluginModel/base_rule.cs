using System;
using System.Linq;
using System.Text;
using DeeGateway.Repository.PluginModel;
using SqlSugar;

namespace DeeGateway.Repository.PluginModel
{

    public partial class base_rule
    {
        public int id { get; set; }

        public string title { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int urlroute_id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int condition_type { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public Condition[] condition_value { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public Condition[] extractor { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int enable { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string url { get; set; }
        public string urlroute_name { get; set; }
    }
}
