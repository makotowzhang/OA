using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class TableDataModel
    {
        public int Total { get; set; }

        public IEnumerable<object> List { get; set; }

        public TableDataModel(int total, IEnumerable<object> list)
        {
            Total = total;
            List = list;
        }
    }
}
