using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.PMModel;
using AutoMapper;
using Data.PMData;
using Entity;
using Model.EnumModel;
using Aspose.Words;
using Aspose.Cells;
using Aspose.Slides;
using System.IO;
using Ionic.Zip;


namespace Business.PMBusiness
{
    public class DataLibraryBusiness
    {
        DataLibraryData data = new DataLibraryData();

        public DataLibraryModel GetFileById(int id)
        {
            using (DataProvider dp = new DataProvider())
            { 
                return Mapper.Map<DataLibraryModel>(data.GetFileById(dp, id));
            }
        }

        public List<DataLibraryModel> GetFileList(DataLibraryFilter filter,out int total)
        {
            using (DataProvider dp = new DataProvider())
            {
                return data.GetFileList(dp, filter, out total);
            }
        }

        public bool Delete(List<DataLibraryModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var dl in list)
                {
                    if (!dl.Id.HasValue)
                    {
                        continue;
                    }
                    var entity = data.GetFileById(dp, dl.Id.Value);
                    if (entity == null)
                    {
                        continue;
                    }
                    entity.IsDel = true;
                    entity.UpdateUser = dl.UpdateUser;
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

        public bool Save(DataLibraryModel model,string rootPath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.FileTempPath)&&model.FileTempPath!= "###")
                {
                    model.FileExtension = Path.GetExtension(model.FileTempPath);
                    string ext = model.FileExtension.ToUpper();
                    if (ext == ".DOC" || ext == ".DOCX")
                    {
                        model.FileType = FileType.Word.ToString();
                    }
                    if (ext == ".XLS" || ext == ".XLSX")
                    {
                        model.FileType = FileType.Excel.ToString();
                    }
                    if (ext == ".PPT" || ext == ".PPTX")
                    {
                        model.FileType = FileType.PowerPoint.ToString();
                    }
                    if (ext == ".PDF")
                    {
                        model.FileType = FileType.PDF.ToString();
                    }
                    string dir = "";
                    string fp = "";
                    using (DataProvider dp = new DataProvider())
                    {
                        fp = "\\Upload\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + dp.System_DicItem.FirstOrDefault(m => m.Id == model.FileClassify).ItemDesc + "\\";
                        dir = rootPath.TrimEnd('\\') + fp;
                    }
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    model.FilePath = fp + Guid.NewGuid().ToString() + model.FileExtension;
                    File.Copy(rootPath.TrimEnd('\\') + model.FileTempPath, rootPath.TrimEnd('\\') + model.FilePath);
                    File.Delete(rootPath.TrimEnd('\\') + model.FileTempPath);
                    if (model.FileType == FileType.PDF.ToString())
                    {
                        model.CanPreview = true;
                        model.PreviewPath = model.FilePath;
                    }
                    else
                    {
                        model.CanPreview = false;
                    }
                }
                if (model.Id.HasValue)
                {
                    using (DataProvider dp = new DataProvider())
                    {
                        var entity = data.GetFileById(dp,model.Id.Value);
                        entity.FileName = model.FileName;
                        entity.FileClassify = model.FileClassify;
                        entity.UpdateUser = model.UpdateUser;
                        entity.UpdateTime = DateTime.Now;
                        if (!string.IsNullOrWhiteSpace(model.FileTempPath) && model.FileTempPath != "###")
                        {
                            entity.FileType = model.FileType;
                            entity.FileExtension = model.FileExtension;
                            entity.FilePath = model.FilePath;
                            entity.CanPreview = model.CanPreview;
                        }
                        dp.SaveChanges(); 
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.FileTempPath))
                    {
                        return false;
                    }
                    model.CreateTime = DateTime.Now;
                    model.IsDel = false;
                    using (DataProvider dp = new DataProvider())
                    {
                        var entity = Mapper.Map<PM_DataLibrary>(model);
                        dp.PM_DataLibrary.Add(entity);
                        dp.SaveChanges();
                        model.Id = entity.Id;
                    }
                }
                if (!string.IsNullOrWhiteSpace(model.FileTempPath))
                {
                    Task.Run(()=> 
                    {
                        string previewPath =  "\\Preview\\"+Path.GetFileNameWithoutExtension(model.FilePath)+".pdf";
                        if (model.FileType == FileType.Word.ToString())
                        {
                            new Document(rootPath.TrimEnd('\\') + model.FilePath).Save(rootPath.TrimEnd('\\')+previewPath, Aspose.Words.SaveFormat.Pdf);
                           
                        }
                        if (model.FileType == FileType.Excel.ToString())
                        {
                            new Workbook(rootPath.TrimEnd('\\') + model.FilePath).Save(rootPath.TrimEnd('\\')+previewPath, Aspose.Cells.SaveFormat.Pdf);
                        }
                        if (model.FileType == FileType.PowerPoint.ToString())
                        {
                            new Presentation(rootPath.TrimEnd('\\') + model.FilePath).Save(rootPath.TrimEnd('\\')+previewPath, Aspose.Slides.Export.SaveFormat.Pdf);
                        }
                        using (DataProvider dp = new DataProvider())
                        {
                            var entity = data.GetFileById(dp, model.Id.Value);
                            entity.PreviewPath = previewPath;
                            entity.CanPreview = true;
                            dp.SaveChanges();
                        }
                    });
                }
                return true;
            }
            catch
            {
                throw;
            }
        }


        public byte[] DownloadFile(List<DataLibraryModel> list, string rootPath)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            foreach (var file in list)
            {
                if (!File.Exists(rootPath.TrimEnd('\\') + "\\" + file.FilePath.TrimStart('\\')))
                {
                    throw new Exception("服务器文件丢失,请联系管理员");
                }
            }
            if (list.Count == 1)
            { 
                return File.ReadAllBytes(rootPath.TrimEnd('\\') + "\\" + list[0].FilePath.TrimStart('\\'));
            }
            else
            {
                using (ZipFile zips = new ZipFile(Encoding.UTF8))
                {
                    foreach (DataLibraryModel data in list)
                    {
                        ZipEntry entry = zips.AddFile(rootPath.TrimEnd('\\') + "\\" + data.FilePath.TrimStart('\\'));
                        entry.FileName = data.FileClassifyName + "\\" + data.FileName + data.FileExtension;
                    }
                    using (MemoryStream stream = new MemoryStream())
                    {
                       
                        zips.Save(stream);
                        byte[] vs = new byte[stream.Length];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(vs, 0, vs.Length);
                        return vs;
                    }
                }
            }
        }


    }
}
