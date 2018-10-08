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
    }
}
