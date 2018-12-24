using Model.EnumModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.PMModel;
using Business.PMBusiness;
using Model.SystemModel;


namespace OAWeb.Controllers
{
    public class ThingsTodoController : BaseController
    {
        private ThingsTodoBusiness business = new ThingsTodoBusiness();

        // GET: Employee
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(ThingsTodoFilter filter)
        {
            filter.CreateUser = CurrentUser.Id;
            var data = business.GetList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetModel(int? id)
        {
            return Json(business.GetModel(id));
        }

        [LogFilter("编辑", "代办事项", LogActionType.Operation)]
        public ActionResult Save(ThingsTodoModel mod)
        {
            try
            {
                if (!mod.Id.HasValue)
                {
                    mod.CreateUser = CurrentUser.Id;
                }
                else
                {
                    mod.UpdateUser = CurrentUser.Id;
                }
                return Json(new JsonMessage(business.Save(mod)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        [LogFilter("删除", "代办事项", LogActionType.Operation)]
        public ActionResult Delete(List<ThingsTodoModel> mod)
        {
            try
            {
                return Json(new JsonMessage(business.Delete(mod)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


    }
}