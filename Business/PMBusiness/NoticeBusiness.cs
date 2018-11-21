using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.PMData;
using Model.PMModel;
using Model.EnumModel;
using AutoMapper;
using Entity;
using Common;


namespace Business.PMBusiness
{
    public class NoticeBusiness
    {
        private NoticeData data = new NoticeData();

        public NoticeModel GetModel(int id)
        {
            using (DataProvider dp = new DataProvider())
            {
                NoticeModel model = Mapper.Map<NoticeModel>(data.GetNotById(dp, id));
                return model;
            }
        }

        public NoticeModel GetNotByIdForShow(int id)
        {
            using (DataProvider dp = new DataProvider())
            {
                NoticeModel model = data.GetNotByIdForShow(dp,id);
                return model;
            }
        }

        public bool Save(NoticeModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetNotById(dp, model.Id);
                if (entity == null)
                {
                    model.IsDel = false;
                    model.CreateTime = DateTime.Now;
                    dp.PM_Notice.Add(Mapper.Map<PM_Notice>(model));
                }
                else
                {
                    entity.IsEnabled = model.IsEnabled;
                    entity.NoticeContent = model.NoticeContent;
                    entity.NoticeType = model.NoticeType;
                    entity.Priority = model.Priority;
                    entity.Remarks = model.Remarks;
                    entity.NoticeTitle = model.NoticeTitle;
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

        public List<NoticeModel> GetNotList(NoticeFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetNotList(dp, filter, out total, isPage);
                return list;
            }
        }

        public bool Delete(List<NoticeModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var not in list)
                {
                    var entity = data.GetNotById(dp, not.Id);
                    if (entity == null)
                    {
                        continue;
                    }
                    entity.IsDel = true;
                    entity.UpdateUser = not.UpdateUser;
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
