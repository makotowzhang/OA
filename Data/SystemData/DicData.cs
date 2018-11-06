using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.EnumModel;

namespace Data.SystemData
{
    public class DicData
    {
        public List<System_DicGroup> GetDicGroupList(DataProvider dp, DicGroupCode? code)
        {
            var list = dp.System_DicGroup.Where(m => !m.IsDel);
            if (code != null)
            {
                string temp = code.ToString();
                list = list.Where(m => m.GroupCode == temp);
            }
            return list.ToList();
        }

        public List<System_DicItem> GetDicItemList(DataProvider dp, DicGroupCode? code,bool all=false)
        {
            var list = dp.System_DicItem.Where(m => !m.IsDel);
            if (!all)
            {
                list = list.Where(m => m.IsEnabled);
            }
            if (code != null)
            {
                string groupCode = code.ToString();
                list = list.Where(m => m.GroupCode == groupCode);
            }
            return list.OrderBy(m=>m.Sort).ThenBy(m=>m.ItemDesc).ToList();
        }
    }
}
