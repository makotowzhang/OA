using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SystemModel;
using Entity;
using Data.SystemData;

namespace Business.SystemBusiness
{
    public class SysMessageBusiness
    {
        private SysMessageData Data=new SysMessageData();
        public List<SysMessageModel> GetSysMessageList(SysMessageFilter filter, out int total)
        {
            using (DataProvider dp = new DataProvider())
            {
                return Data.GetSysMessageList(dp, filter, out total);
            }
        }

        public int GetNotReadCount(Guid userId)
        {
            using (DataProvider dp = new DataProvider())
            {
                return Data.GetNotReadCount(dp, userId);
            }
        }

        public bool MarkRead(List<Guid> messageId,Guid markUser)
        {
            using (DataProvider dp = new DataProvider())
            {
                DateTime now = DateTime.Now;
                foreach (var mk in dp.System_MessageReceiver.Where(m => messageId.Contains(m.MessageId) && m.ToUser == markUser))
                {
                    if (!mk.IsRead)
                    {
                        mk.FirstReadTime = now;
                    }
                    mk.LastReadTime = now;
                    mk.IsRead = true;
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
