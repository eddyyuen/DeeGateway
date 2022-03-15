layui.define(['table', 'form'], function (exports) {

    var obj = {
        IsEmpty: function (o) {
            if (o == undefined || o == null || o == "") {
                return true;
            } else {
                return false;
            }
        },
        GetQueryString: function (key) {
            var reg = new RegExp("(^|&)" + key + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);  //获取url中"?"符后的字符串并正则匹配
            var context = "";
            if (r != null)
                context = r[2];
            reg = null;
            r = null;
            return context == null || context == "" || context == "undefined" ? "" : context;
        },
        ConditionType: function (val) {
            var text = "";
            switch (val) {
                case 1:
                    text = "单个";
                    break;
                case 2:
                    text = "全部";
                    break;
            }
            return text;
        }
        ,
        ConditionValueType: function (val) {
            var text = "";
            switch (val) {
                case 1:
                    text = "URI";
                    break;
                case 2:
                    text = "HEADER";
                    break;
                case 3:
                    text = "QUERY";
                    break;
                case 4:
                    text = "BODY";
                    break;
                case 5:
                    text = "IP";
                    break;
                default:
                    text = "";
                    break;

            }
            return text;

        },
        ConditionValueHandle: function (val) {
            var text = "";
            switch (val) {
                case 1:
                    text = "MATCH";
                    break;
                case 2:
                    text = "NOT_MATCH";
                    break;
                case 3:
                    text = "CONTAINS";
                    break;
                case 4:
                    text = "EQUAL_TO";
                    break;
                case 5:
                    text = "GREATER_THAN";
                    break;
                case 5:
                    text = "LESS_THAN";
                    break;
                default:
                    text = "";
                    break;
            }
            return text;

        },
        ExtractorType: function (val) {

        }
    }
    exports('utils', obj);
});