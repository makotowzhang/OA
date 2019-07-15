using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.PMData;
using Entity;
using Model.PMModel;


namespace Business.PMBusiness
{
    public class SysWorkBusiness
    {
        private SysWorkData data = new SysWorkData();

        public SysWorkModel GetModel(int id)
        {
            using (DataProvider dp = new DataProvider())
            {
                return Mapper.Map<SysWorkModel>(data.GetSysWorkById(dp, id));
            }
        }

        public bool CheckSysWorkExist(SysWorkModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                if (!model.Id.HasValue)
                {
                    return dp.PM_SysWork.Count(m => m.WorkNo == model.WorkNo) > 0;
                }
                else
                {
                    return dp.PM_SysWork.Count(m => m.WorkNo == model.WorkNo&&m.Id!=model.Id.Value) > 0;
                }
            }
        }

        public bool Save(SysWorkModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                PM_SysWork entity = null;
                if (model.Id.HasValue)
                {
                    entity = data.GetSysWorkById(dp, model.Id.Value);
                }
                if (entity == null)
                {
                    model.IsDel = false;
                    model.CreateTime = DateTime.Now;
                    dp.PM_SysWork.Add(Mapper.Map<PM_SysWork>(model));
                }
                else
                {
                    entity.WorkNo = model.WorkNo;
                    entity.WorkName = model.WorkName;
                    entity.Principal = model.Principal;
                    entity.WorkType = model.WorkType;
                    entity.ProjectLeader = model.ProjectLeader;
                    entity.SpecialConsultant = model.SpecialConsultant;
                    entity.FileName = model.FileName;
                    entity.FilePath = model.FilePath;
                    entity.UpdateUser = model.UpdateUser;
                    entity.UpdateTime = DateTime.Now;
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<SysWorkModel> GetSysWorkList(SysWorkFilter filter, out int total)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetSysWorkList(dp, filter, out total);
                return Mapper.Map<List<SysWorkModel>>(list);
            }
        }


        public bool Delete(List<SysWorkModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var work in list)
                {
                    if (!work.Id.HasValue)
                    {
                        continue;
                    }
                    var entity = data.GetSysWorkById(dp, work.Id.Value);
                    if (entity == null)
                    {
                        continue;
                    }
                    entity.IsDel = true;
                    entity.UpdateUser = work.UpdateUser;
                    entity.UpdateTime = DateTime.Now;
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
