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
                       join rec in dp.System_MessageReceiver on msg.Id equals rec.MessageId
                       join user in dp.System_User on msg.CreateUser equals user.Id
                       where !msg.IsDel && rec.ToUser ==filter.ToUser
                       select
                       new SysMessageModel()
                       {
                           Id=msg.Id,
                           CreateTime=msg.CreateTime,
                           CreateUser=msg.CreateUser,
                           IsRead= rec.IsRead,
                           MsgContent=msg.MsgContent,
                           SendUserName=user.TrueName,
                           MsgTitle=msg.MsgTitle,
                           MsgType=msg.MsgType,
                           ToUser= rec.ToUser,
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
            return dp.System_Message.Count(m => !m.IsDel && dp.System_MessageReceiver.Any(x=>x.MessageId==m.Id&&!x.IsRead&&x.ToUser==userId));
        }


        public bool SendMessage(DataProvider dp, System_Message msg, List<Guid> toUsers)
        {
            msg.Id = Guid.NewGuid();
            dp.System_Message.Add(msg);
            foreach (Guid userId in toUsers)
            {
                dp.System_MessageReceiver.Add(new System_MessageReceiver()
                {
                    MessageId=msg.Id,
                    IsRead=false,
                    ToUser=userId
                });
            }
            return true;
            
        }
    }
}
