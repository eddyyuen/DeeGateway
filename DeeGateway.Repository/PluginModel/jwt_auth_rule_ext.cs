using System;
using System.Linq;
using System.Text;
using DeeGateway.Repository.PluginModel;
using SqlSugar;

namespace DeeGateway.Repository.PluginModel
{

    public partial class jwt_auth_rule_ext :base_rule
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public JwtAuthHandle handle { get; set; }

       
    }
    public class JwtAuthHandle
    {
        public string status { get; set; }
        public string message { get; set; }
        public int returncode { get; set; }
    }
}
