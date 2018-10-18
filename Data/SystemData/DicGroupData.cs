using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Data.SystemData
{
    public class DicGroupData
    {
        public List<System_DicGroup> GetGroupList(DataProvider dp)
        {
            return dp.System_DicGroup.Where(m => !m.IsDel).ToList();
        }
    }
}
