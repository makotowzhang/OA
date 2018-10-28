using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SystemModel;

namespace Model.PMModel
{
    public class DepartmentModel : CommonModel
    {
        public Guid Id { get; set; }
        public string DepName { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsDel { get; set; }
    }

    public class DepartmentFilter : PageModel
    {
        public string DepName { get; set; }
    }
}
