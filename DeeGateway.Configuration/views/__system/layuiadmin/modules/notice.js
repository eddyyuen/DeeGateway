layui.define(['form'], function (exports) {

    var obj = {
        save: function (resp) {
            if (resp.code === 0) {
                layer.msg("保存成功", {
                    icon: 6,
                    time: 2000,
                    shade: 0.5,
                    end: function () {
                        window.location.reload();
                    }
                });
            } else {
                layer.msg("保存失败", {
                    icon: 5,
                    time: 2000,
                    shade: 0.5
                });
            }
        },
        del:function(obj){
            var data = obj.data;
            if (obj.event === 'del') {
                layer.confirm('确定删除？', function (index) {
                    obj.del();
                    layer.close(index);
                });
            }
        }
        
    }
    exports('notice', obj);
});