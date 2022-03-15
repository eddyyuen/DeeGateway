using DeeGateway.Repository.Constant;
using System;
using System.Collections.Generic;
using System.Text;
using static DeeGateway.Repository.Constant.Enums;

namespace DeeGateway.Repository.PluginModel
{

    //public class Condition
    //{
    //    public Enums.ConditionType type { get; set; }
    //    public Handle[] handle { get; set; }
    //}

    public class Condition
    {
        public int order { get; set; }
        public string key { get; set; }
        public ConditionValueType type { get; set; }
        public string value { get; set; }
        public int handle { get; set; }

        public string default_value { get; set; }

    }

}
