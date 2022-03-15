using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DeeGateway.Repository.Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("urlroute_server")]
    public partial class urlroute_server
    {
           public urlroute_server(){


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
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int maxrps {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int server_id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int urlroute_id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:10
           /// Nullable:False
           /// </summary>           
           public int weight {get;set;}

    }
}
