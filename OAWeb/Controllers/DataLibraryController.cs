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
            return View();
        }
    }
}