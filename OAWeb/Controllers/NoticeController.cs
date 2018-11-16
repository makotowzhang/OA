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
    public class NoticeController : BaseController
    {
        private NoticeBusiness business = new NoticeBusiness();
        
        // GET: Department
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetNotList(NoticeFilter filter)
        {
            var data = business.GetNotList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }


        public ActionResult GetNotModel(int id)
        {
            return Json(business.GetModel(id));
        }

        [LogFilter("编辑", "公告管理", LogActionType.Operation)]
        public ActionResult Save(NoticeModel not)
        {
            try
            {
                if (not.Id == null)
                {
                    not.CreateUser = CurrentUser.Id;
                }
                else
                {
                    not.UpdateUser = CurrentUser.Id;
                }
                return Json(new JsonMessage(business.Save(not)));
            }
            catch(Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        [LogFilter("删除", "公告管理", LogActionType.Operation)]
        public ActionResult Delete(List<NoticeModel> not)
        {
            try
            {
                not.ForEach(m => m.UpdateUser = CurrentUser.Id);
                return Json(new JsonMessage(business.Delete(not)));
            }
            catch(Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }
    }
}