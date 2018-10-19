using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.SystemModel;
using Business.SystemBusiness;
using Model.EnumModel;

namespace OAWeb.Controllers
{
    public class DictionaryController : BaseController
    {
        DicBusiness business = new DicBusiness();
        // GET: Dictionary
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDicList()
        {
            var data = business.GetDicGroup(null);
            return EnumJson(data);
        }


        [LogFilter("新增组", "字典管理", LogActionType.Operation)]
        public ActionResult AddGorup(DicGroupModel model)
        {
            try
            {
                if (model.GroupCode == null)
                {
                    return Json(new JsonMessage(false, "字典类型不存在"));
                }
                model.CreateUser = CurrentUser.Id;
                bool success = business.AddDicGroup(model);
                return Json(new JsonMessage(success));
            }
            catch(Exception ex)
            {
                return Json(new JsonMessage(false,ex.Message));
            }
        }
    }
}