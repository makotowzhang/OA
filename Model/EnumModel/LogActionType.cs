using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EnumModel
{
    public enum LogActionType
    {
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Login = 1,
        /// <summary>
        /// 登出
        /// </summary>
        [Description("登出")]
        LogOut = 2,
        /// <summary>
        /// 操作
        /// </summary>
        [Description("业务操作")]
        Operation = 3,
        /// <summary>
        /// 错误
        /// </summary>
        [Description("系统错误")]
        Error = 4,
        /// <summary>
        /// 修改参数
        /// </summary>
        [Description("修改参数")]
        ModifyPara=5
    }
}
