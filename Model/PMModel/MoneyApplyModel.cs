using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EnumModel;
using Model.SystemModel;

namespace Model.PMModel
{
    public class MoneyApplyModel:CommonModel
    {
        public Guid? Id { get; set; }
        public decimal Amount { get; set; }
        public  string AuditStatus { get; set; }
        public string ApplyReason { get; set; }
        public Guid? AuditUser { get; set; }
        public string AuditReason { get; set; }
        public DateTime? AuditTime { get; set; }
        public string MaterielName { get; set; }
        public string CreateUserName { get; set; }
        public string AuditUserName { get; set; }
        public bool IsDel { get; set; }
        public List<Guid> AuditDep { get; set; }

        public List<Guid> AuditUserIds { get; set; }
    }


    public class MoneyApplyFilter : PageModel
    {
        public List<string> AuditStatus { get; set; }

        public string CreateUserName { get; set; }

        public string AuditUserName { get; set; }

        public Guid? CreateUserId { get; set; }

        public Guid? AuditUserId { get; set; }

        public DateTime? CreateBeginTime { get; set; }

        public DateTime? CreateEndTime { get; set; }

        public DateTime? AuditBeginTime { get; set; }

        public DateTime? AuditEndTime { get; set; }

        public ListType ListType { get; set; }
    }
}
