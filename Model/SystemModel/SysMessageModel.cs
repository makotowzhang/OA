using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class SysMessageModel
    {
        public int Id { get; set; }
        public string MsgTitle { get; set; }
        public string MsgContent { get; set; }
        public string MsgType { get; set; }
        public string Url { get; set; }
        public Guid ToUser { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateTime { get; set; }
        public bool IsRead { get; set; }
        public bool IsDel { get; set; }

        public string SendUserName { get; set; }
    }

    public class SysMessageFilter:PageModel
    {
        public string MsgTitle { get; set; }

        public string MsgType { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public Guid? ToUser { get; set; }
    }
}
