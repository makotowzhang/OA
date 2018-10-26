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
    public class DicItemModel
    {
        public Guid? Id { get; set; }

        public string ItemDesc { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DicGroupCode GroupCode { get; set; }

        public string GroupDesc { get; set; }

        public int Sort { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsDel { get; set; }

        public Guid CreateUser { get; set; }

        public DateTime CreateTime { get; set; }

        public Guid? UpdateUser { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
