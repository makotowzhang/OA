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
    public class LeaseApplyBusiness
    {
        private LeaseApplyData data = new LeaseApplyData();

        public LeaseApplyModel GetModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                LeaseApplyModel model = Mapper.Map<LeaseApplyModel>(data.GetLEAModel(dp, id));
                if (model != null)
                {
                    model.AuditDep = dp.PM_Department.Where(m => !m.IsDel &&
                                                            dp.PM_AuditUser.Where(x => x.ApplyId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                          .Select(m => m.Id).ToList();
                    model.AuditUserIds = dp.System_User.Where(m => m.IsDel == false &&
                                                             dp.PM_AuditUser.Where(x => x.ApplyId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                          .Select(m => m.Id).ToList();
                }
                return model;
            }
        }

        public bool Save(LeaseApplyModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetLEAModel(dp, model.Id);
                if (entity == null)
                {

                    model.Id = Guid.NewGuid();
                    model.AuditStatus = AuditStatus.WaitAudit.ToString();
                    model.IsDel = false;
                    model.CreateTime = DateTime.Now;
                    dp.PM_LeaseApply.Add(Mapper.Map<PM_LeaseApply>(model));
                    if (model.AuditDep.IsNotNullAndCountGtZero())
                    {
                        model.AuditDep.ForEach(m => dp.PM_AuditUser.Add(new PM_AuditUser()
                        {
                            ApplyId=model.Id,
                            AuditId=m
                        }));
                    }
                    if (model.AuditUserIds.IsNotNullAndCountGtZero())
                    {
                        model.AuditUserIds.ForEach(m => dp.PM_AuditUser.Add(new PM_AuditUser()
                        {
                            ApplyId = model.Id,
                            AuditId = m
                        }));
                    }
                }
                else
                {
                    entity.LeaseId = model.LeaseId;
                    entity.LeaseBeginTime = model.LeaseBeginTime;
                    entity.LeaseEndTime = model.LeaseEndTime;
                    entity.ApplyReason = model.ApplyReason;
                    entity.UpdateUser = model.UpdateUser;
                    entity.UpdateTime = DateTime.Now;
                    dp.PM_AuditUser.RemoveRange(dp.PM_AuditUser.Where(m=>m.ApplyId==entity.Id));
                    if (model.AuditDep.IsNotNullAndCountGtZero())
                    {
                        model.AuditDep.ForEach(m => dp.PM_AuditUser.Add(new PM_AuditUser()
                        {
                            ApplyId = model.Id,
                            AuditId = m
                        }));
                    }
                    if (model.AuditUserIds.IsNotNullAndCountGtZero())
                    {
                        model.AuditUserIds.ForEach(m => dp.PM_AuditUser.Add(new PM_AuditUser()
                        {
                            ApplyId = model.Id,
                            AuditId = m
                        }));
                    }
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

        public List<LeaseApplyModel> GetLEAList(LeaseApplyFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetLEAList(dp, filter, out total, isPage);
                return list;
            }
        }

        public bool Delete(List<LeaseApplyModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var dep in list)
                {
                    if (!dep.Id.HasValue)
                    {
                        continue;
                    }
                    var entity = data.GetLEAModel(dp, dep.Id.Value);
                    if (entity == null)
                    {
                        continue;
                    }
                    entity.IsDel = true;
                    entity.UpdateUser = dep.UpdateUser;
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

        public bool Audit(LeaseApplyModel model)
        {
            if (model == null || !model.Id.HasValue)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetLEAModel(dp, model.Id);
                entity.AuditStatus = model.AuditStatus;
                entity.AuditReason = model.AuditReason;
                entity.AuditTime = DateTime.Now;
                entity.AuditUser = model.AuditUser;
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

        public bool Return(LeaseApplyModel model)
        {
            if (model == null || !model.Id.HasValue)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetLEAModel(dp, model.Id);
                entity.ActualReturnTime = model.ActualReturnTime;
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
