using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entity;
using Model.SystemModel;

namespace Business.SystemBusiness
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<System_User, UserModel>();
                m.CreateMap<UserModel, System_User>();
                m.CreateMap<System_Role, RoleModel>();
                m.CreateMap<RoleModel, System_Role>();
                m.CreateMap<System_Menu, MenuModel>();
                m.CreateMap<MenuModel, System_Menu>();
                m.CreateMap<System_Log, LogModel>();
                m.CreateMap<LogModel, System_Log>();
                m.CreateMap<System_Message, SysMessageModel>();
                m.CreateMap<SysMessageModel, System_Message>();
            });
        }
    }
}
