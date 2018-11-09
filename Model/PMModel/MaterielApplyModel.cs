using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EnumModel;
using Model.SystemModel;

namespace Model.PMModel
{
    public class MaterielApplyModel:CommonModel
    {
        public Guid? Id { get; set; }
        public int MaterielId { get; set; }
        public int Quantity { get; set; }
        public  string AuditStatus { get; set; }
        public string ApplyReason { get; set; }
        public Guid? AuditUser { get; set; }
        public string AuditReason { get; set; }
        public DateTime? AuditTime { get; set; }
        public string MaterielName { get; set; }
        public string CreateUserName { get; set; }
        public string AuditUserName { get; set; }

        public List<Guid> AuditDep { get; set; }

        public List<Guid> AuditUserIds { get; set; }
    }


    public class MaterielApplyFilter : PageModel
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
    }
}
