function GetUrlParameter(key) {
    var para = location.search;
    var theRequest = new Object();
    if (para.indexOf("?") != -1) {
        var str = para.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0].toUpperCase()] = unescape(strs[i].split("=")[1]);
        }
    }
    if (key != null && key != "") {
        return theRequest[key.toUpperCase()];
    }
    else {
        return theRequest;
    }
}

function GetActionAuthorize(vueEx) {
    axios.post("GetPageAuthorize", {}).then(function (response) {
        for (var m in vueEx.ActionPermission) {
            for (var n in response.data) {
                if (m == response.data[n]) {
                    vueEx.ActionPermission[m] = true;
                }
            }
        }
        setTimeout(window.onresize, 0);
    });
}

function ActionCommonHandle(resData, vueEx, successMsg, failedMsg, successCallback) {
    if (successMsg == "") {
        successMsg = resData.msg;
    }
    if (failedMsg == "") {
        failedMsg = resData.msg;
    }
    if (resData.success) {
        vueEx.$message({
            type: 'success',
            message: successMsg,
            showClose: true
        });
        if (successCallback != undefined) {
            successCallback();
        }
    }
    else {
        vueEx.$message({
            type: 'info',
            message: resData.msg == "" ? failedMsg : resData.msg,
            showClose: true
        });
    }
}

function ErrorCommonHandle(vueEx) {
    vueEx.$message({
        type: 'error',
        message: '服务器异常',
        showClose: true
    });
    console.log(error);
}

function DateFormat(date, format) {
    if (date == null) return "";
    //date = date.replace('T', ' ');
    date = eval("new " + date.replace("/", "").replace("/",""));


    var map = {
        "M": date.getMonth() + 1, //月份
        "d": date.getDate(), //日
        "h": date.getHours(), //小时
        "H": date.getHours(), //小时
        "m": date.getMinutes(), //分
        "s": date.getSeconds(), //秒
        "q": Math.floor((date.getMonth() + 3) / 3), //季度
        "S": date.getMilliseconds() //毫秒
    };
    format = format.replace(/([yMdhHmsqS])+/g, function (all, t) {
        var v = map[t];
        if (v !== undefined) {
            if (all.length > 1) {
                v = '0' + v;
                v = v.substr(v.length - 2);
            }
            return v;
        }
        else if (t === 'y') {
            return (date.getFullYear() + '').substr(4 - all.length);
        }
        return all;
    });
    return format;
}

//页面左击右击事件监听，去除Tab的菜单
window.addEventListener("load", function () {
    document.body.addEventListener("click", function () {
        top.$app.TabMenuPosition.visible = false;
        top.$app.MessageDiv.visible = false;
    });
    document.body.addEventListener("contextmenu", function () {
        top.$app.TabMenuPosition.visible = false;
    });
});
axios.interceptors.request.use(function (request) {
    var menuId = GetUrlParameter("MenuId");
    if (menuId == null || menuId == "") {
        return request;
    }
    if (request.method == "post") {
        if (request.data == null) {
            request.data = {};
        }
        if (request.data.menuId == null) {
            request.data.menuId = menuId;
        }
    }
    return request;
});


axios.interceptors.response.use(function (response) {
    if (response.request.responseURL.indexOf("Login/Index") != -1) {
        new Vue().$alert('登录会话过期请重新登录', '提示', {
            confirmButtonText: '确定',
            callback: function () {
                top.location.href = "/Login/Index";
            }
        });
    }
    return response;    
});

