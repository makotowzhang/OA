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
    public class MaterielController : BaseController
    {
        private MaterielBusiness business = new MaterielBusiness();

        // GET: Employee
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMatList(MaterielFilter filter)
        {
            var data = business.GetMatList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetMatModel(int id)
        {
            return Json(business.GetModel(id));
        }

        [LogFilter("编辑", "物料管理", LogActionType.Operation)]
        public ActionResult Save(MaterielModel mat)
        {
            try
            {
                if (mat.Id==0)
                {
                    mat.CreateUser = CurrentUser.Id;
                }
                else
                {
                    mat.UpdateUser = CurrentUser.Id;
                }
                return Json(new JsonMessage(business.Save(mat)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        [LogFilter("删除", "物料管理", LogActionType.Operation)]
        public ActionResult Delete(List<MaterielModel> mat)
        {
            try
            {
                mat.ForEach(m => m.UpdateUser = CurrentUser.Id);
                return Json(new JsonMessage(business.Delete(mat)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }

        public ActionResult GetAllMat()
        {
            return Json(business.GetAllMat());
        }

        public ActionResult CheckMatCode(string matCode,int? id)
        {
            return Json(new JsonMessage(business.CheckMatCodeExsit(matCode,id)));
        }
    }
}