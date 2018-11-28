using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EnumModel
{
    public enum  DicGroupCode
    {
        /// <summary>
        /// 资料库类别
        /// </summary>
        DataLibraryType=1,

        /// <summary>
        /// 物料类型
        /// </summary>
        MaterielType=2,

        /// <summary>
        /// 公告类别
        /// </summary>
        NoticeType=3,

        /// <summary>
        /// 租借物品类型
        /// </summary>
        LeaseType=4,

        /// <summary>
        /// 估价目的
        /// </summary>
        ValuationObjective = 5,

        /// <summary>
        /// 估价方法
        /// </summary>
        ValuationMethods=6,

        /// <summary>
        /// 收费状态
        /// </summary>
        ChargeStatus=7
    }
}
