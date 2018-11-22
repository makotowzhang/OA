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
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDicList()
        {
            var data = business.GetDicGroup(null);
            return EnumJson(data);
        }


        public ActionResult GetGroupItemList(DicGroupCode groupCode)
        {
            return EnumJson(business.GetDicItem(groupCode));
        }

        [LogFilter("编辑组", "字典管理", LogActionType.Operation)]
        public ActionResult AddGorup(DicGroupModel model)
        {
            try
            {
                if (model.GroupCode == null)
                {
                    return Json(new JsonMessage(false, "字典类型不存在"));
                }
                bool success;
                if (model.Id == null)
                {
                    model.CreateUser = CurrentUser.Id;
                    success = business.AddDicGroup(model);
                }
                else
                {
                    model.UpdateUser = CurrentUser.Id;
                    success = business.EditDicGroup(model);
                }
                return Json(new JsonMessage(success));
            }
            catch(Exception ex)
            {
                return Json(new JsonMessage(false,ex.Message));
            }
        }

        [LogFilter("编辑项", "字典管理", LogActionType.Operation)]
        public ActionResult AddItem(DicItemModel model)
        {
            try
            {
                bool success;
                if (model.Id == null)
                {
                    model.CreateUser = CurrentUser.Id;
                    success = business.AddDicItem(model);
                }
                else
                {
                    model.UpdateUser = CurrentUser.Id;
                    success = business.EditDicItem(model);
                }
                return Json(new JsonMessage(success));
            }
            catch (Exception ex)
            {
                return Json(new JsonMessage(false, ex.Message));
            }
        }

        [LogFilter("删除项", "字典管理", LogActionType.Operation)]
        public ActionResult DeleteItem(List<DicItemModel> dicItems)
        {
            try
            {
                if (dicItems != null)
                {
                    dicItems.ForEach(m=>m.UpdateUser=CurrentUser.Id);
                }
                bool success = business.DeleteItems(dicItems);
                return Json(new JsonMessage(success));
            }
            catch (Exception ex)
            {
                return Json(new JsonMessage(false, ex.Message));
            }
        }
    }
}