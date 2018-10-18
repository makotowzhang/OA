using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class DicItemModel
    {
        public Guid Id { get; set; }
        public string ItemDesc { get; set; }
        public Guid GroupId { get; set; }
        public bool IsDel { get; set; }
        public Guid CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid? UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
