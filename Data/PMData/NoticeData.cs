using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.PMModel;
using Entity;
using Common;

namespace Data.PMData
{
    public class NoticeData
    {
        public PM_Notice GetNotById(DataProvider dp, int? notId)
        {
            return dp.PM_Notice.FirstOrDefault(m => m.Id == notId);
        }

        public List<NoticeModel> GetNotList(DataProvider dp, NoticeFilter filter, out int total, bool IsPage = true)
        {
            var list = from not in dp.PM_Notice.Where(m => !m.IsDel)
                       join user1 in dp.System_User.Where(m => m.IsDel == false) on not.CreateUser equals user1.Id
                       join user2 in dp.System_User.Where(m => m.IsDel == false) on not.UpdateUser equals user2.Id
                       join dic in dp.System_DicItem.Where(m => !m.IsDel) on not.NoticeType equals dic.Id
                       select new NoticeModel
                       {
                           Id = not.Id,
                           CreateTime = not.CreateTime,
                           CreateUser = not.CreateUser,
                           CreateUserName = user1.TrueName,
                           IsEnabled = not.IsEnabled,
                           NoticeTitle = not.NoticeTitle,
                           NoticeType = not.NoticeType,
                           NoticeTypeName = dic.ItemDesc,
                           Priority = not.Priority,
                           Remarks = not.Remarks,
                           UpdateTime = not.UpdateTime,
                           UpdateUser = not.UpdateUser,
                           UpdateUserName = user2.TrueName
                       };
            if (filter.NoticeTitle.IsNotNullOrWhiteSpace())
            {
                list = list.Where(m => m.NoticeTitle.Contains(filter.NoticeTitle));
            }
            if (filter.NoticeType.IsNotNullAndCountGtZero())
            {
                list = list.Where(m => filter.NoticeType.Contains(m.NoticeType));
            }

            list = list.OrderByDescending(m=>m.Priority).ThenByDescending(m => m.CreateTime);
            total = list.Count();
            if (IsPage)
            {
                return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
            }
            else
            {
                return list.ToList();
            }
        }

    }
}
