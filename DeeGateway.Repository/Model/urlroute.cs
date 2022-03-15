using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DeeGateway.Repository.Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("urlroute")]
    public partial class urlroute
    {
           public urlroute(){


           }
           /// <summary>
           /// Desc:
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int enable {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string hash_pattern {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int id {get;set;}

        public string urlroute_name { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string url {get;set;}

    }
}
