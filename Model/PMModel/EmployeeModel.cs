using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PMModel
{
    public class EmployeeModel: CommonModel
    {
        public Guid? Id { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? RelateUserId { get; set; }
        public Guid DepartmentId { get; set; }
        public bool IsDel { get; set; }
        public string RelateUserName { get; set; }
        public List<Guid> RelateRoleIds { get; set; }
        public bool IsRelateUser { get; set; }
        public string DepName { get; set; }

    }

    public class EmployeeFilter : PageModel
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public List<Guid> DepList { get; set; }
    }
}
