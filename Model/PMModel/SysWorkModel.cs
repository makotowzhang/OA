using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PMModel
{
    public class SysWorkModel: CommonModel
    {
        public int? Id { get; set; }
        public string WorkNo { get; set; }
        public string WorkName { get; set; }
        public string Principal { get; set; }
        public string WorkType { get; set; }
        public string ProjectLeader { get; set; }
        public string SpecialConsultant { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public bool IsDel { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
    }

    public class SysWorkFilter : PageModel
    {
        public string WorkNo { get; set; }

        public string WorkName { get; set; }

        public string UserName { get; set; }

        public Guid? UserId { get; set; }

        public List<string> WorkType { get; set; }
    }
}
