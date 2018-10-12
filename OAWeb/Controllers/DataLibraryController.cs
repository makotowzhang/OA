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
                FileType=FileType.Word.ToString(),
                FilePath= "Upload\\A8A7C90B-0578-4E18-ABBE-695EF0C36835.docx"

            };
            business.TypeChange(model, Server.MapPath("~"));
            return Content("");
        }
    }
}