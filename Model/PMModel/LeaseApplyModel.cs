using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EnumModel;
using Model.SystemModel;

namespace Model.PMModel
{
    public class LeaseApplyModel : CommonModel
    {
        public Guid? Id { get; set; }
        public int LeaseId { get; set; }
        public DateTime LeaseBeginTime { get; set; }
        public DateTime LeaseEndTime { get; set; }
        public DateTime? ActualReturnTime { get; set; }
        public  string AuditStatus { get; set; }
        public string ApplyReason { get; set; }
        public Guid? AuditUser { get; set; }
        public string AuditReason { get; set; }
        public DateTime? AuditTime { get; set; }
        public string LeaseName { get; set; }
        public string CreateUserName { get; set; }
        public string AuditUserName { get; set; }
        public bool IsDel { get; set; }
        public List<Guid> AuditDep { get; set; } = new List<Guid>();

        public List<Guid> AuditUserIds { get; set; } = new List<Guid>();
    }


    public class LeaseApplyFilter : PageModel
    {
        public List<string> AuditStatus { get; set; }

        public List<int> LeaseIds { get; set; }

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
