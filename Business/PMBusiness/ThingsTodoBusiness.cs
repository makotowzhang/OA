using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.PMData;
using Model.PMModel;
using AutoMapper;
using Entity;


namespace Business.PMBusiness
{
    public class ThingsTodoBusiness
    {
        private ThingsTodoData data = new ThingsTodoData();

        public ThingsTodoModel GetModel(int? id)
        {
            using (DataProvider dp = new DataProvider())
            {
                return Mapper.Map<ThingsTodoModel>(data.GetEntityById(dp, id));
            }
        }

        public bool Save(ThingsTodoModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetEntityById(dp, model.Id);
                if (entity == null)
                {
                    model.IsAlert = false;
                    model.CreateTime = DateTime.Now;
                    dp.PM_ThingsTodo.Add(Mapper.Map<PM_ThingsTodo>(model));
                }
                else
                {
                    entity.TodoDate = model.TodoDate;
                    entity.TodoThings = model.TodoThings;
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

        public List<ThingsTodoModel> GetList(ThingsTodoFilter filter, out int total,bool isPage=true)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetList(dp, filter, out total, isPage);
                return Mapper.Map<List<ThingsTodoModel>>(list);
            }
        }

        public List<ThingsTodoModel> GetWeekThingsTodo(Guid CreateUser)
        {
            DateTime now = DateTime.Now.Date;
            ThingsTodoFilter filter = new ThingsTodoFilter
            {
                BeginTime = now,
                EndTime = now.AddDays(6),
                CreateUser = CreateUser
            };
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetList(dp, filter,out int total, false);
                return Mapper.Map<List<ThingsTodoModel>>(list);
            }

        }

        public List<ThingsTodoModel> GetDailyThingsTodo(Guid CreateUser)
        {
            DateTime now = DateTime.Now.Date;
            ThingsTodoFilter filter = new ThingsTodoFilter
            {
                BeginTime = now,
                EndTime = now,
                CreateUser = CreateUser,
                IsAlert=false
            };
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetList(dp, filter, out int total, false);
                return Mapper.Map<List<ThingsTodoModel>>(list);
            }

        }

        public bool DailyAlert(int? id)
        {
            using (DataProvider dp = new DataProvider())
            {
                if (!id.HasValue)
                {
                    return false;
                }
                var entity = data.GetEntityById(dp, id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsAlert = true;
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

        public bool Delete(List<ThingsTodoModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var mod in list)
                {
                    if (mod.Id==0)
                    {
                        continue;
                    }
                    var entity = data.GetEntityById(dp, mod.Id);
                    if (entity == null)
                    {
                        continue;
                    }
                    dp.PM_ThingsTodo.Remove(entity);
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
