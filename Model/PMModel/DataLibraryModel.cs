using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PMModel
{
    public class DataLibraryModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public EnumModel.FileType FileType { get; set; }
        public string FilePath { get; set; }
        public bool CanPreview { get; set; }
        public string PreviewPath { get; set; }
        public bool IsDel { get; set; }
        public Guid CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid? UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class DataLibraryFilter:PageModel
    {
        public string FileName { get; set; }
    }
}
