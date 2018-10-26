using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EnumModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Model.SystemModel
{
    public class DicGroupModel
    {
        public Guid? Id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DicGroupCode? GroupCode { get; set; }
        public string GroupDesc { get; set; }
        public bool IsDel { get; set; }
        public List<DicItemModel> Items { get; set; }
        public Guid CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid? UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
