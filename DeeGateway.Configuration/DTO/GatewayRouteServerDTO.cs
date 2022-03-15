using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Configuration.DTO
{
    public class GatewayRouteServerDTO
    {
        public string Host
        {
            get;
            set;
        }

        public int Weight
        {
            get;
            set;
        }

        public bool Available
        {
            get;
            set;
        }

        public int MaxRps
        {
            get;
            set;
        }
    }
}
