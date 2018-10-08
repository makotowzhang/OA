using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.SystemModel;

namespace Data.SystemData
{
    public class SysMessageData
    {
        public List<SysMessageModel> GetSysMessageList(DataProvider dp, SysMessageFilter filter, out int total)
        {
            var view = from msg in dp.System_Message
                       join user in dp.System_User on msg.ToUser equals user.Id
                       where !msg.IsDel && msg.ToUser ==filter.ToUser
                       select
                       new SysMessageModel()
                       {
                           Id=msg.Id,
                           CreateTime=msg.CreateTime,
                           CreateUser=msg.CreateUser,
                           IsRead=msg.IsRead,
                           MsgContent=msg.MsgContent,
                           SendUserName=user.TrueName,
                           MsgTitle=msg.MsgTitle,
                           MsgType=msg.MsgType,
                           ToUser=msg.ToUser,
                           Url=msg.Url
                       };
            if (!string.IsNullOrWhiteSpace(filter.MsgTitle))
            {
                view = view.Where(m => m.MsgTitle.Contains(filter.MsgTitle));
            }
            if (!string.IsNullOrWhiteSpace(filter.MsgType))
            {
                view = view.Where(m => m.MsgType==filter.MsgType);
            }
            if (filter.IsRead!=null)
            {
                view = view.Where(m => m.IsRead==filter.IsRead);
            }
            if (filter.BeginTime!=null)
            {
                view = view.Where(m => m.CreateTime>=filter.BeginTime);
            }
            if (filter.EndTime != null)
            {
                view = view.Where(m => m.CreateTime <= filter.EndTime);
            }
            total = view.Count();
            return view.OrderByDescending(m => m.CreateTime).ThenBy(m => m.IsRead).Skip(filter.Skip).Take(filter.PageSize).ToList();
        }

        public int GetNotReadCount(DataProvider dp, Guid userId)
        {
            return dp.System_Message.Count(m => !m.IsDel && !m.IsRead && m.ToUser == userId);
        }
    }
}
