using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PMModel
{
    public class NoticeModel: CommonModel
    {
        public int? Id { get; set; }
        public Guid NoticeType { get; set; }
        public string NoticeTitle { get; set; }
        public string NoticeContent { get; set; }
        public string Remarks { get; set; }
        public int Priority { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDel { get; set; }

        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
        public string NoticeTypeName { get; set; }
    }

    public class NoticeFilter : PageModel
    {
        public string NoticeTitle { get; set; }
        public List<Guid> NoticeType { get; set; }
      
    }
}
