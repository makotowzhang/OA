using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EnumModel
{
    public enum ReportStatus
    {
        /// <summary>
        /// 待提交
        /// </summary>
        WaitSubmit=0,
        /// <summary>
        /// 待审核
        /// </summary>
        WaitAudit = 1,

        /// <summary>
        /// 审核通过
        /// </summary>
        Passed = 2,

        /// <summary>
        /// 驳回
        /// </summary>
        Reject = 3,

        /// <summary>
        /// 撤销
        /// </summary>
        Revoke = 4
    }
}
