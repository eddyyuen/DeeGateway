using DeeGateway.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Repository.CustomModel
{
    public class urlroute_server_ext :urlroute_server
    {
        public string server_name { get; set; }
        public string uri { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1000
        /// Nullable:False
        /// </summary>           
        public int max_connections { get; set; }


        public string urlroute_name { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string url { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string hash_pattern { get; set; }
    }
}
