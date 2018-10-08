using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Model.SystemModel
{
    public class MenuModel
    {
        public Guid Id { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public string MenuType { get; set; }
        public string ActionCode { get; set; }
        public string ActionDesc { get; set; }
        public string IconClass { get; set; }
        public Guid? ParentId { get; set; }
        public int? Sort { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsDel { get; set; }
        public Guid? CreateUser { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public List<MenuModel> Children { get; set; }
    }

}
