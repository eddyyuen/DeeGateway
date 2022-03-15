layui.define(['table', 'utils', 'form'], function (exports) {
    var utils = layui.utils;
    var obj = {
        ConditionValueHtml: function (item) {
            if (utils.IsEmpty(item)) {
                return '';
            }
            var html = '';
            html += '<span class="layui-badge  layui-bg-green">' + utils.ConditionValueType(item.type);
            if (!utils.IsEmpty(item.key)) {
                html += '</span> <span class="layui-badge layui-bg-cyan"> ' + item.key;
            }
            html += '</span> <span class="layui-badge"> ' + utils.ConditionValueHandle(item.handle)
                + '</span> <span class="layui-badge  layui-bg-gray"> ' + item.value + '</span><br/>';
            return html;
        },
        ExtractorHtml: function (item) {
            if (utils.IsEmpty(item)) {
                return '';
            }
            var html = '';
            html += '<span class="layui-badge  layui-bg-green">' + utils.ConditionValueType(item.type)
            if (!utils.IsEmpty(item.key)) {
                //   html += '</span> <span class="layui-badge layui-bg-cyan"> [' + item.key+']';
            }
            switch (item.type) {
                case 1: //URI
                    html += '</span> <span class="layui-badge"> MATCH';
                    html += '</span> <span class="layui-badge  layui-bg-gray"> ' + item.value + '</span><br/>';
                    break;
                case 2:
                    //text = "HEADER";
                    html += '</span> <span class="layui-badge"> KEY=';
                    html += '</span> <span class="layui-badge layui-bg-gray"> ' + item.key + '';
                    html += '</span> <span class="layui-badge-rim"> ' + item.value + '</span><br/>';
                    break;
                case 3:
                    //text = "QUERY";
                    html += '</span> <span class="layui-badge"> PARAMS=';
                    html += '</span> <span class="layui-badge  layui-bg-gray"> ' + item.value + '</span><br/>';
                    break;
                case 4:
                    //text = "BODY";
                    html += '</span> <span class="layui-badge"> MATCH';
                    html += '</span> <span class="layui-badge  layui-bg-gray"> ' + item.value + '</span><br/>';
                    break;
                case 5:
                    //text = "IP";
                    html += '</span> ';
                    if (!utils.IsEmpty(item.value)) {
                        html += ' <span class="layui-badge  layui-bg-gray"> ' + item.value + '</span><br/>';
                    }
                    break;
                case 6:
                    //text = "server_uri";
                    html += '</span> ';
                    if (!utils.IsEmpty(item.value)) {
                        html += ' <span class="layui-badge  layui-bg-gray"> ' + item.value + '</span><br/>';
                    }
                    break;

            }

            return html;
        },
        IndexHtml: function (index) {
            return '<span class="layui-badge layui-bg-orange">' + index + '</span>';
        },
        IndexExtractorHtml: function (index) {
            return '<span class="layui-badge layui-bg-orange">{' + index + '}</span>';
        }, JwtAuthHandlerHtml: function (handle) {
            if (utils.IsEmpty(handle)) {
                return '';
            }
            var html = '';
            html += '<span class="layui-badge  layui-bg-green">Status</span> <span class="layui-badge  layui-bg-gray"> '
                + handle.status + '</span><br/>';
            html += '<span class="layui-badge  layui-bg-green">Message</span> <span class="layui-badge  layui-bg-gray"> '
                + handle.message + '</span><br/>';
            html += '<span class="layui-badge  layui-bg-green">ReturnCode</span> <span class="layui-badge  layui-bg-gray"> '
                + handle.returncode + '</span><br/>';

            return html;
        },
        RewriteHandleHtml: function (handle) {
            if (utils.IsEmpty(handle)) {
                return '';
            }
            var html = '';
            html += '<span class="layui-badge  layui-bg-green">Url</span> <span class="layui-badge  layui-bg-gray"> '
                + handle.url + '</span><br/>';

            return html;
        },
        HeaderHandleHtml: function (handle) {
            if (utils.IsEmpty(handle)) {
                return '';
            }
            var html = '';
            html += '<span class="layui-badge  layui-bg-green">Key</span> <span class="layui-badge  layui-bg-gray"> '
                + handle.key + '</span> ';
            html += '<span class="layui-badge  layui-bg-green">Value</span> <span class="layui-badge  layui-bg-gray"> '
                + handle.value + '</span><br/>';

            return html;
        },
        JwtAuthHandleHtml: function (handle) {
            if (utils.IsEmpty(handle)) {
                return '';
            }
            var html = '';
            html += '<span class="layui-badge  layui-bg-green">status</span> <span class="layui-badge  layui-bg-gray"> '
                + handle.status + '</span> ';
            html += '<span class="layui-badge  layui-bg-green">ReturnCode</span> <span class="layui-badge  layui-bg-gray"> '
                + handle.returncode + '</span><br/>';
            html += '<span class="layui-badge  layui-bg-green">Messsage</span> <span class="layui-badge  layui-bg-gray"> '
                + handle.message + '</span> ';

            return html;
        }
    };
    exports('html', obj);
});