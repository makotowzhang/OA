using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class RoleModel
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public int? Sort { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsDel { get; set; }
        public Guid? CreateUser { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class RoleFilter: PageModel
    {
        public string RoleName { get; set; }
    }
}
