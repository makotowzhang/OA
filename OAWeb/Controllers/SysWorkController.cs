using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.PMModel;
using Business.PMBusiness;
using Model.SystemModel;
using Model.EnumModel;

namespace OAWeb.Controllers
{
    public class SysWorkController : BaseController
    {
        private SysWorkBusiness business = new SysWorkBusiness();

        // GET: Department
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetSysWorkList(SysWorkFilter filter)
        {
            filter.UserId = CurrentUser.Id;
            filter.UserName = CurrentUser.UserName;
            var data = business.GetSysWorkList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }



        public ActionResult GetSysWorkModel(int id)
        {
            return Json(business.GetModel(id));
        }

        [LogFilter("编辑", "招标代理管理", LogActionType.Operation)]
        public ActionResult Save(SysWorkModel work)
        {
            try
            {
                if (business.CheckSysWorkExist(work))
                {
                    return Json(new JsonMessage(false, "办公编号已存在！"));
                }
                if (work.Id == null)
                {
                    work.CreateUser = CurrentUser.Id;
                }
                else
                {
                    work.UpdateUser = CurrentUser.Id;
                }
                return Json(new JsonMessage(business.Save(work)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        [LogFilter("删除", "招标代理管理", LogActionType.Operation)]
        public ActionResult Delete(List<SysWorkModel> work)
        {
            try
            {
                work.ForEach(m => m.UpdateUser = CurrentUser.Id);
                return Json(new JsonMessage(business.Delete(work)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        public ActionResult DownloadFile(int? id)
        {
            if (!id.HasValue)
            {
                return Content("参数错误");
            }
            var model  = business.GetModel(id.Value);
            if (model == null||string.IsNullOrWhiteSpace(model.FileName)||!System.IO.File.Exists(Server.MapPath("~" + model.FilePath)))
            {
                return Content("报告不存在");
            }
            return File(Server.MapPath("~"+model.FilePath), "application/octet-stream", model.FileName);
        }
    }
}