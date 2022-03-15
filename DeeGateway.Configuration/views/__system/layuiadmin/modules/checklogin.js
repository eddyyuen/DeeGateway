layui.define(['table', 'form'], function (exports) {

    var ApiUrl = layui.sessionData('DeeGateway');

    if (ApiUrl.AccessToken == undefined || ApiUrl.AccessToken == null || ApiUrl.AccessToken == "") {
        self.location.href = "/__system/user/login.html";
    }

    exports('checklogin', {});
});