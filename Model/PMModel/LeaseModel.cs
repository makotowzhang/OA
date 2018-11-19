using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SystemModel;

namespace Model.PMModel
{
    public class LeaseModel : CommonModel
    {
        public int? Id { get; set; }
        public string LeaseName { get; set; }
        public Guid LeaseType { get; set; }
        public int Sort { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDel { get; set; }
        public string LeaseTypeDesc { get; set; }
    }

    public class LeaseFilter : PageModel
    {
        public string LeaseName { get; set; }
        public List<Guid> LeaseType { get; set; }
    }
}
