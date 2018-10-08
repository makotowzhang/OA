using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class AuthorizeModel
    {
        public Guid RoleId { get; set; }

        public List<Guid> MenuIdList { get; set; }

        public Guid CreateUser { get; set; }
    }
}
