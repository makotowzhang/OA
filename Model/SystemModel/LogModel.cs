using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class LogModel
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string MenuName { get; set; }
        public string IpAddress { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }
        public Guid? DoUser { get; set; }
        public DateTime? DoTime { get; set; }
        public string DoUserName { get; set; }

    }

    public class LogFilter : PageModel
    {
        public string ActionName { get; set; }

        public string MenuName { get; set; }

        public string ActionType { get; set; }

        public string DoUserName { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
