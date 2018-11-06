using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.SystemData;
using Model.SystemModel;
using Entity;
using AutoMapper;
using Model.EnumModel;

namespace Business.SystemBusiness
{
    public class DicBusiness
    {
        DicData data = new DicData();

        public bool AddDicGroup(DicGroupModel model)
        {
            System_DicGroup entity = Mapper.Map<System_DicGroup>(model);
            using (DataProvider dp = new DataProvider())
            {
                if (dp.System_DicGroup.Count(m => !m.IsDel && m.GroupCode == entity.GroupCode) > 0)
                {
                    throw new Exception("字典组已存在！");
                }
                entity.Id = Guid.NewGuid();
                entity.CreateTime = DateTime.Now;
                entity.IsDel = false;
                dp.System_DicGroup.Add(entity);
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

        public bool AddDicItem(DicItemModel model)
        {
            System_DicItem entity = Mapper.Map<System_DicItem>(model);
            using (DataProvider dp = new DataProvider())
            {
                entity.Id = Guid.NewGuid();
                entity.CreateTime = DateTime.Now;
                entity.IsDel = false;
                dp.System_DicItem.Add(entity);
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

        public bool EditDicGroup(DicGroupModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                System_DicGroup entity = dp.System_DicGroup.FirstOrDefault(m => m.Id == model.Id);
                if (entity == null)
                {
                    return false;
                }
                entity.GroupDesc = model.GroupDesc;
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUser = model.UpdateUser;
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

        public bool EditDicItem(DicItemModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                System_DicItem entity = dp.System_DicItem.FirstOrDefault(m => m.Id == model.Id);
                if (entity == null)
                {
                    return false;
                }
                entity.ItemDesc = model.ItemDesc;
                entity.Sort = model.Sort;
                entity.IsEnabled = model.IsEnabled;
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUser = model.UpdateUser;
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


        public List<DicGroupModel> GetDicGroup(DicGroupCode? code)
        {
            using (DataProvider dp = new DataProvider())
            {
                List<DicGroupModel> groupList = Mapper.Map<List<DicGroupModel>>(data.GetDicGroupList(dp, code));
                List<DicItemModel> itemList = Mapper.Map<List<DicItemModel>>(data.GetDicItemList(dp, code));
                groupList.ForEach(m=>
                {
                    m.Items = itemList.Where(x => x.GroupCode == m.GroupCode).ToList();
                });
                return groupList;
            }
        }

        public List<DicItemModel> GetDicItem(DicGroupCode? code)
        {
            using (DataProvider dp = new DataProvider())
            {
                List<DicItemModel> itemList = Mapper.Map<List<DicItemModel>>(data.GetDicItemList(dp, code,false));
                return itemList;
            }
        }

        public bool DeleteItems(List<DicItemModel> dicItems)
        {
            if (dicItems == null || dicItems.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var m in dicItems)
                {
                    var entity = dp.System_DicItem.FirstOrDefault(x => x.Id == m.Id);
                    if (entity != null)
                    {
                        entity.IsDel = true;
                        entity.UpdateTime = DateTime.Now;
                        entity.UpdateUser = m.UpdateUser;
                    }
                }
                dp.SaveChanges();
                return true;
            }
        }

    }
}
