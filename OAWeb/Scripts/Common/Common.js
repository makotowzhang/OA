var $globalVue = new Vue();

var $Msg = {
    info: function (str, showClose) {
        if (showClose == undefined) {
            showClose = true;
        }
        $globalVue.$message({
            message: str,
            showClose: showClose,
            type: 'info'
        })
    },
    success: function (str, showClose) {
        if (showClose == undefined) {
            showClose = true;
        }
        $globalVue.$message({
            message: str,
            showClose: showClose,
            type: 'success'
        })
    },
    warning: function (str, showClose) {
        if (showClose == undefined) {
            showClose = true;
        }
        $globalVue.$message({
            message: str,
            showClose: showClose,
            type: 'warning'
        })
    },
    error: function (str, showClose) {
        if (showClose == undefined) {
            showClose = true;
        }
        $globalVue.$message({
            message: str,
            showClose: showClose,
            type: 'error'
        })
    },
    iconMsg: function (str, iconClass, showClose) {
        if (showClose == undefined) {
            showClose = true;
        }
        $globalVue.$message({
            message: str,
            showClose: showClose,
            iconClass: iconClass
        })
    }
};

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

//日期格式化
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

function compareTime(startDate, endDate) {
    if (startDate.length > 0 && endDate.length > 0) {
        var startDateTemp = startDate.split(" ");
        var endDateTemp = endDate.split(" ");

        var arrStartDate = startDateTemp[0].split("-");
        var arrEndDate = endDateTemp[0].split("-");

        var arrStartTime = startDateTemp[1].split(":");
        var arrEndTime = endDateTemp[1].split(":");

        var allStartDate = new Date(arrStartDate[0], arrStartDate[1], arrStartDate[2], arrStartTime[0], arrStartTime[1], arrStartTime[2]);
        var allEndDate = new Date(arrEndDate[0], arrEndDate[1], arrEndDate[2], arrEndTime[0], arrEndTime[1], arrEndTime[2]);

        if (allStartDate.getTime() >= allEndDate.getTime()) {  
            return false;
        } else {
            return true;
        }
    } else {
        return false;
    }
}   

function compareDate(checkStartDate, checkEndDate) {
    var arys1 = new Array();
    var arys2 = new Array();
    if (checkStartDate != null && checkEndDate != null) {
        arys1 = checkStartDate.split('-');
        var sdate = new Date(arys1[0], parseInt(arys1[1] - 1), arys1[2]);
        arys2 = checkEndDate.split('-');
        var edate = new Date(arys2[0], parseInt(arys2[1] - 1), arys2[2]);
        if (sdate > edate) {
            return false;
        } else {
            return true;
        }
    }
}     

function compareCalendar(startDate, endDate) {
    if (startDate.indexOf(" ") != -1 && endDate.indexOf(" ") != -1) {
        //包含时间，日期  
        return compareTime(startDate, endDate);
    } else {
        //不包含时间，只包含日期  
        return compareDate(startDate, endDate);
    }
}   

//属性自动赋值
function AutoMapper(source, data) {
    for (var m in source) {
        source[m] = data[m];
    }
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

