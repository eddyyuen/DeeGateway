using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DeeGateway.Repository.Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("plugins")]
    public partial class plugins
    {
        public plugins()
        {


        }
        /// <summary>
        /// Desc:0-disable 1-enable
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int enable { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1000
        /// Nullable:False
        /// </summary>           
        public int level { get; set; }

        /// <summary>
        /// Desc:plugin name
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string plugin_name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string type { get; set; }
        public string setting { get; set; }
        public string description { get; set; }

    }
}
