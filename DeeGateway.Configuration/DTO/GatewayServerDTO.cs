using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Configuration.DTO
{
    public class GatewayServerDTO
    {
        public string Host
        {
            get;
            set;
        }

        public int MaxConnections
        {
            get;
            set;
        }

        public bool Available
        {
            get;
            set;
        }
    }
}
