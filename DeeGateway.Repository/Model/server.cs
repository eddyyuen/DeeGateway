using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DeeGateway.Repository.Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("server")]
    public partial class server
    {
        public server()
        {


        }
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
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1000
        /// Nullable:False
        /// </summary>           
        public int max_connections { get; set; }

        public string server_name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string uri { get; set; }

    }
}
