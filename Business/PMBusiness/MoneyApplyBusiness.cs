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
    public class MoneyApplyBusiness
    {
        private MoneyApplyData data = new MoneyApplyData();

        public MoneyApplyModel GetModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                MoneyApplyModel model = Mapper.Map<MoneyApplyModel>(data.GetPMAModel(dp, id));
                if (model != null)
                {
                    model.AuditDep = dp.PM_Department.Where(m => !m.IsDel &&
                                                            dp.PM_AuditUser.Where(x => x.ApplyId == m.Id).Select(x => x.AuditId).Contains(m.Id))
                                                          .Select(m => m.Id).ToList();
                    model.AuditUserIds = dp.System_User.Where(m => m.IsDel == false &&
                                                             dp.PM_AuditUser.Where(x => x.ApplyId == m.Id).Select(x => x.AuditId).Contains(m.Id))
                                                          .Select(m => m.Id).ToList();
                }
                return model;
            }
        }

        public bool Save(MoneyApplyModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetPMAModel(dp, model.Id);
                if (entity == null)
                {

                    model.Id = Guid.NewGuid();
                    model.AuditStatus = AuditStatus.WaitAudit.ToString();
                    model.IsDel = false;
                    model.CreateTime = DateTime.Now;
                    dp.PM_MoneyApply.Add(Mapper.Map<PM_MoneyApply>(model));
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
                    entity.Amount = model.Amount;
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

        public List<MoneyApplyModel> GetPMAList(MoneyApplyFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetPMAList(dp, filter, out total, isPage);
                return list;
            }
        }

        public bool Delete(List<MoneyApplyModel> list)
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
                    var entity = data.GetPMAModel(dp, dep.Id.Value);
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

        public bool Audit(MoneyApplyModel model)
        {
            if (model == null || !model.Id.HasValue)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetPMAModel(dp, model.Id);
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
    }
}
