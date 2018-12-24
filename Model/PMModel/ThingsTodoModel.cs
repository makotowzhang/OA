using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SystemModel;

namespace Model.PMModel
{
    public class ThingsTodoModel : CommonModel
    {
        public int? Id { get; set; }
        public System.DateTime TodoDate { get; set; }
        public string TodoThings { get; set; }

        public bool IsAlert { get; set; }
    }

    public class ThingsTodoFilter : PageModel
    {
        public string TodoThings { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public Guid? CreateUser { get; set; }

        public bool? IsAlert { get; set; }
    }
}
