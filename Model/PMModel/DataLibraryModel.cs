using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EnumModel;

namespace Model.PMModel
{
    public class DataLibraryModel:CommonModel
    {
        public int? Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileType { get; set; }
        public Guid FileClassify { get; set; }
        public string FilePath { get; set; }
        public bool CanPreview { get; set; }
        public string PreviewPath { get; set; }
        public bool IsDel { get; set; }

        public string CreateUserName { get; set; }

        public string FileClassifyName { get; set; }

        public string FileTempPath { get; set; }

    }

    public class DataLibraryFilter:PageModel
    {
        public string FileName { get; set; }

        public List<Guid> FileClassify { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
