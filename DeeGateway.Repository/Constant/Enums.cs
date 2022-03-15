using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Repository.Constant
{
    public class Enums
    {
        public enum IsEnable
        {
            ENABLE = 1,
            DISABLE = 0
        }

        public enum ConditionType
        {
            /// <summary>
            /// 一个符合
            /// </summary>
            ONE = 1,
            /// <summary>
            /// 全部符合
            /// </summary>
            ALL = 2
        }

        public enum ConditionValueType
        {

            URI = 1,
            HEADER= 2,
            QUERY = 3,
            BODY =4 ,
            IP =5,
           // SERVER_URI = 6
        }
        public enum ConditionValueHandle
        {
            /// <summary>
            /// 符合
            /// </summary>
            MATCH = 1,
            /// <summary>
            /// 不符合
            /// </summary>
            NOT_MATCH =2 ,
            /// <summary>
            /// 包含
            /// </summary>
            CONTAINS = 3,
            /// <summary>
            /// 等于
            /// </summary>
            EQUAL_TO = 4,
            /// <summary>
            /// 大于
            /// </summary>
            GREATER_THAN = 5,
            /// <summary>
            /// 小于
            /// </summary>
            LESS_THAN =6
        }
    }
}
