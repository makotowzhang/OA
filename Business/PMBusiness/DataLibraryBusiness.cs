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

namespace Business.PMBusiness
{
    public class DataLibraryBusiness
    {
        DataLibraryData data = new DataLibraryData();

        public void TypeChange(DataLibraryModel model,string rootPath)
        {
            string filePath = rootPath.TrimEnd('\\') + "\\" + model.FilePath.TrimStart('\\');
            if (model.FileType == FileType.Word)
            {
                Document doc = new Document(filePath);
                Aspose.Words.Saving.ImageSaveOptions iso = new Aspose.Words.Saving.ImageSaveOptions(Aspose.Words.SaveFormat.Jpeg)
                {
                    Resolution = 128,
                    PrettyFormat = true,
                    UseAntiAliasing = true
                };
                Guid guid = Guid.NewGuid();
                for (int i = 0; i < doc.PageCount; i++)
                {
                    iso.PageIndex = i;
                    doc.Save(rootPath.TrimEnd('\\') + "\\Preview\\" + guid + "\\image" + i + ".jpg", iso);
                }
            }
            if (model.FileType == FileType.Excel)
            {
                Workbook workbook = new Workbook(filePath);
                workbook.Save(rootPath.TrimEnd('\\') + "\\Preview\\image.pdf", Aspose.Cells.SaveFormat.Pdf);
            }
        }
    }
}
