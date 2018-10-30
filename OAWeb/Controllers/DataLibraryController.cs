using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.PMBusiness;
using Model.PMModel;
using Model.EnumModel;
using Model.SystemModel;

namespace OAWeb.Controllers
{
    public class DataLibraryController : BaseController
    {
        DataLibraryBusiness business = new DataLibraryBusiness();
        // GET: DataLibrary
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileUpload()
        {
            var httpFile = Request.Files;
            if (httpFile == null || httpFile.Count == 0)
            {
                return Content("");
            }
            string tempFilePath ="\\Upload\\temp\\";
            if (!System.IO.Directory.Exists(Server.MapPath("~" + tempFilePath)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~" + tempFilePath));
            }
            tempFilePath += Guid.NewGuid().ToString() + System.IO.Path.GetExtension(httpFile[0].FileName);
            httpFile[0].SaveAs(Server.MapPath("~" + tempFilePath));
            return Content(tempFilePath);
        }

        public ActionResult GetFileList(DataLibraryFilter filter)
        {
            var data = business.GetFileList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetFileModel(int id)
        {
            return Json(business.GetFileById(id));
        }

        [LogFilter("编辑", "资料库管理", LogActionType.Operation)]
        public ActionResult Save(DataLibraryModel model)
        {
            try
            {
                if (!model.Id.HasValue)
                {
                    model.CreateUser = CurrentUser.Id;
                }
                else
                {
                    model.UpdateUser = CurrentUser.Id;
                }
                return Json(new JsonMessage(business.Save(model,Server.MapPath("~"))));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        [LogFilter("删除", "资料库管理", LogActionType.Operation)]
        public ActionResult Delete(List<DataLibraryModel> model)
        {
            try
            {
                model.ForEach(m => m.UpdateUser = CurrentUser.Id);
                return Json(new JsonMessage(business.Delete(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }

        public ActionResult Download(string list)
        {
            Session["DownloadFlag"] = false;
            try
            {
                List<DataLibraryModel> model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DataLibraryModel>>(list);
                string fileName = "";
                if (model == null || model.Count == 0)
                {
                    throw new Exception("未选择下载的文件！");
                }
                fileName = model[0].FileName;
                if (model.Count() == 1)
                {
                    fileName += model[0].FileExtension;
                }
                else
                {
                    fileName = $"_{ fileName}_等文件.zip";
                }
                return File(business.DownloadFile(model, Server.MapPath("~")), "application/octet-stream", fileName);
            }
            catch (Exception e)
            {
                return Content(@"<script>top.$Msg.error('" + e.Message + "')</script>");
            }
            finally
            {
                Session["DownloadFlag"] = true;
            }
        }

        public ActionResult GetDownloadStatus()
        {
            while (Session["DownloadFlag"]==null||!(bool)Session["DownloadFlag"])
            {
                System.Threading.Thread.Sleep(200);
            }
            Session["DownloadFlag"] = null;
            return Content("");
        }
    }
}