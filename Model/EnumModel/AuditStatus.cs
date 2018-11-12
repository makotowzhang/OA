using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EnumModel
{
    public enum AuditStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        WaitAudit=1,

        /// <summary>
        /// 审核通过
        /// </summary>
        Passed=2,

        /// <summary>
        /// 驳回
        /// </summary>
        Reject=3,

        /// <summary>
        /// 撤销
        /// </summary>
        Revoke=4
    }

    public enum ListType
    {
        /// <summary>
        /// 所有数据
        /// </summary>
        AllList=1,
        /// <summary>
        /// 个人申请
        /// </summary>
        Personal=2,
        /// <summary>
        /// 审核人
        /// </summary>
        Auditor=3
    }
}
