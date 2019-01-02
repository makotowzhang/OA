using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entity;
using Model.SystemModel;
using Model.PMModel;
using Model.RMModel;

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
                m.CreateMap<DicGroupModel, System_DicGroup>();
                m.CreateMap<System_DicGroup,DicGroupModel>();

                m.CreateMap<DicItemModel, System_DicItem>();
                m.CreateMap<System_DicItem, DicItemModel>();

                m.CreateMap<DepartmentModel, PM_Department>();
                m.CreateMap<PM_Department, DepartmentModel>();

                m.CreateMap<EmployeeModel, PM_Employee>();
                m.CreateMap<PM_Employee, EmployeeModel>();

                m.CreateMap<PM_DataLibrary, DataLibraryModel>();
                m.CreateMap<DataLibraryModel, PM_DataLibrary>();

                m.CreateMap<PM_Materiel, MaterielModel>();
                m.CreateMap<MaterielModel, PM_Materiel>();

                m.CreateMap<PM_MaterielApply, MaterielApplyModel>();
                m.CreateMap<MaterielApplyModel, PM_MaterielApply>();

                m.CreateMap<PM_MoneyApply, MoneyApplyModel>();
                m.CreateMap<MoneyApplyModel, PM_MoneyApply>();

                m.CreateMap<PM_Notice, NoticeModel>();
                m.CreateMap<NoticeModel, PM_Notice>();

                m.CreateMap<PM_Lease, LeaseModel>();
                m.CreateMap<LeaseModel, PM_Lease>();

                m.CreateMap<PM_LeaseApply, LeaseApplyModel>();
                m.CreateMap<LeaseApplyModel, PM_LeaseApply>();

                m.CreateMap<RM_HouseReport, HouseReportModel>();
                m.CreateMap<HouseReportModel, RM_HouseReport>();

                m.CreateMap<RM_AreaReport, AreaReportModel>();
                m.CreateMap<AreaReportModel, RM_AreaReport>();

                m.CreateMap<RM_AssetsReport, AssetsReportModel>();
                m.CreateMap<AssetsReportModel, RM_AssetsReport>();

                m.CreateMap<PM_ThingsTodo, ThingsTodoModel>();
                m.CreateMap<ThingsTodoModel, PM_ThingsTodo>();

                m.CreateMap<View_Achievements, AchievementsModel>();
                m.CreateMap<AchievementsModel, View_Achievements>();
            });
        }
    }
}
