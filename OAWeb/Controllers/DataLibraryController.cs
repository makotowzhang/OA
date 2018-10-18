using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.PMBusiness;
using Model.PMModel;
using Model.EnumModel;

namespace OAWeb.Controllers
{
    public class DataLibraryController : BaseController
    {
        DataLibraryBusiness business = new DataLibraryBusiness();
        // GET: DataLibrary
        public ActionResult Index()
        {
            DataLibraryModel model = new DataLibraryModel
            {
                //FileType=FileType.Word,
                //FilePath= "Upload\\A8A7C90B-0578-4E18-ABBE-695EF0C36835.docx"
                FileType = FileType.Excel,
                FilePath = "Upload\\亨通MES4.0项目需求说明书-解析属性维护_V1.0-20180917.xls"

            };
            business.TypeChange(model, Server.MapPath("~"));
            return Content("");
        }
    }
}