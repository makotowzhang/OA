using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.SystemModel;
using Common;

namespace Data.SystemData
{
    public class LoginData
    {
        public List<System_User> CheckUser(DataProvider dp,LoginModel model)
        {
            string md5Password = MD5Encrypt.MD5Encrypt64(model.Password);
            return dp.System_User.Where(m => m.UserName == model.UserName
                                        && m.Password == md5Password
                                        && m.IsDel == false ).ToList();
        }
    }
}
