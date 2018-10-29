using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.PMModel;

namespace Data.PMData
{
    public class DataLibraryData
    {
        public PM_DataLibrary GetFileById(DataProvider dp, int id)
        {
            return dp.PM_DataLibrary.FirstOrDefault(m => m.Id == id);
        }

        public List<DataLibraryModel> GetFileList(DataProvider dp, DataLibraryFilter filter, out int total, bool isPage = true)
        {
            var list = from dl in dp.PM_DataLibrary.Where(m => m.IsDel == false)
                       join user in dp.System_User.Where(m => m.IsDel == false) on dl.CreateUser equals user.Id
                       join fc in dp.System_DicItem.Where(m => m.IsDel == false) on dl.FileClassify equals fc.Id
                       select new DataLibraryModel()
                       {
                           Id=dl.Id,
                           CanPreview=dl.CanPreview,
                           CreateTime=dl.CreateTime,
                           CreateUser=dl.CreateUser,
                           CreateUserName=user.TrueName,
                           FileClassify=dl.FileClassify.Value,
                           FileClassifyName=fc.ItemDesc,
                           FileExtension=dl.FileExtension,
                           FileName=dl.FileName,
                           FilePath=dl.FilePath,
                           FileType=dl.FileType,
                           PreviewPath=dl.PreviewPath
                       };
            if (!string.IsNullOrWhiteSpace(filter.FileName))
            {
                list = list.Where(m => m.FileName.Contains(filter.FileName));
            }
            if (filter.FileClassify!=null&&filter.FileClassify.Count>0)
            {
                list = list.Where(m => filter.FileClassify.Contains(m.FileClassify));
            }
            if (filter.BeginTime.HasValue)
            {
                list = list.Where(m => m.CreateTime >= filter.BeginTime.Value);
            }
            if (filter.EndTime.HasValue)
            {
                list = list.Where(m => m.CreateTime <= filter.EndTime.Value);
            }
            total = list.Count();
            if (isPage)
            {
                return list.OrderByDescending(m=>m.CreateTime).Skip(filter.Skip).Take(filter.PageSize).ToList();
            }
            else
            {
                return list.OrderByDescending(m => m.CreateTime).ToList();
            }
        }
    }
}
