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
using System.IO;

namespace Business.PMBusiness
{
    public class DataLibraryBusiness
    {
        DataLibrary data = new DataLibrary();

        public void TypeChange(DataLibraryModel model,string rootPath)
        {
            //Stream stream = new MemoryStream(Convert.FromBase64String("PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4KPExpY2Vuc2U+CiAgPERhdGE+CiAgICA8TGljZW5zZWRUbz5MaWNlbnNlZTwvTGljZW5zZWRUbz4KICAgIDxFbWFpbFRvPmxpY2Vuc2VlQGVtYWlsLmNvbTwvRW1haWxUbz4KICAgIDxMaWNlbnNlVHlwZT5EZXZlbG9wZXIgT0VNPC9MaWNlbnNlVHlwZT4KICAgIDxMaWNlbnNlTm90ZT5MaW1pdGVkIHRvIDEwMDAgZGV2ZWxvcGVyLCB1bmxpbWl0ZWQgcGh5c2ljYWwgbG9jYXRpb25zPC9MaWNlbnNlTm90ZT4KICAgIDxPcmRlcklEPjc4NDM3ODU3Nzg1PC9PcmRlcklEPgogICAgPFVzZXJJRD4xMTk3ODkyNDM3OTwvVXNlcklEPgogICAgPE9FTT5UaGlzIGlzIGEgcmVkaXN0cmlidXRhYmxlIGxpY2Vuc2U8L09FTT4KICAgIDxQcm9kdWN0cz4KICAgICAgPFByb2R1Y3Q+QXNwb3NlLlRvdGFsIFByb2R1Y3QgRmFtaWx5PC9Qcm9kdWN0PgogICAgPC9Qcm9kdWN0cz4KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl0aW9uVHlwZT4KICAgIDxTZXJpYWxOdW1iZXI+e0YyQjk3MDQ1LTFCMjktNEIzRi1CRDUzLTYwMUVGRkExNUFBOX08L1NlcmlhbE51bWJlcj4KICAgIDxTdWJzY3JpcHRpb25FeHBpcnk+MjA5OTEyMzE8L1N1YnNjcmlwdGlvbkV4cGlyeT4KICAgIDxMaWNlbnNlVmVyc2lvbj4zLjA8L0xpY2Vuc2VWZXJzaW9uPgogIDwvRGF0YT4KICA8U2lnbmF0dXJlPlFYTndiM05sTGxSdmRHRnNJRkJ5YjJSMVkzUWdSbUZ0YVd4NTwvU2lnbmF0dXJlPgo8L0xpY2Vuc2U+"));
            //stream.Seek(0L, SeekOrigin.Begin);
            //new Aspose.Words.License().SetLicense(stream);
            string filePath = rootPath.TrimEnd('\\') + "\\" + model.FilePath.TrimStart('\\');
            if (model.FileType == FileType.Word.ToString())
            {
                Document doc = new Document(filePath);
                Aspose.Words.Saving.ImageSaveOptions iso = new Aspose.Words.Saving.ImageSaveOptions(SaveFormat.Jpeg)
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
        }
    }
}
