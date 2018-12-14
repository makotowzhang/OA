using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.SystemBusiness;
using Business.PMBusiness;
using Newtonsoft.Json;
using System.IO;
using Common;

namespace OAWeb.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public UserModel CurrentUser
        {
            get
            {
                if (string.IsNullOrWhiteSpace(User.Identity.Name))
                {
                    return null;
                }
                return new LoginBusiness().GetUserInfoById(Guid.Parse(User.Identity.Name));
            }
        }

        public ActionResult GetPageAuthorize(Guid menuId)
        {
            return Json(new AuthorizeBusiness().GetAuthorizeAction(menuId, CurrentUser.UserRole).Select(m => m.ActionCode));
        }

        public ActionResult GetDepListForSelect()
        {
            return Json(new DepartmentBusiness().GetAllDep());
        }

        public ActionResult CommonUploadFile(string path)
        {
            if (Request.Files == null || Request.Files.Count == 0)
            {
                return Json(new JsonMessage(false));
            }
            else
            {
                try
                {
                    var file = Request.Files[0];
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string dir = "~/Upload/";
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        dir += path.TrimStart('/');
                        dir = dir.TrimEnd('/') + "/";
                    }
                    if (!Directory.Exists(Server.MapPath(dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath(dir));
                    }
                    file.SaveAs(Server.MapPath(dir + fileName));
                    return Json(new JsonMessage(true, dir.Replace("~", "") + fileName));
                }
                catch(Exception e)
                {
                    return Json(new JsonMessage(false, e.Message));
                }
            }
        }

        public ActionResult CommonDownloadFile(string path, string fileName)
        {
            if (!path.StartsWith("~"))
            {
                path = "~" + path;
            }
            FileStream file;
            try
            {
                file = System.IO.File.OpenRead(Server.MapPath(path));
            }
            catch
            {
                return Content("文件未找到");
            }
            return File(file, "application/octet-stream", fileName);
        }

        public EnumJsonResult EnumJson(object data)
        {
            EnumJsonResult result = new EnumJsonResult();
            result.Data = data;
            return result;
        }

        public ActionResult GetCurrentUser()
        {
            var user = CurrentUser;
            user.Password = null;
            return Json(user);
        }

        public ActionResult GetReportQRCode(Guid? ReportId,string ControllerName) {
            ReportId = ReportId ?? Guid.Empty;
            var url = Request.Url.ToString().Replace(Request.Url.PathAndQuery, "") + "/" + ControllerName + "/ReportMobileDetail?reportId=" + ReportId.Value;
            return File(QRCodeHelper.GetQRCodeByteArray(url), "image/Jpeg");
        }
    }

    public class EnumJsonResult : ActionResult
    {
        public object Data
        {
            get;
            set;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            if (this.Data != null)
            {
                response.Write(JsonConvert.SerializeObject(Data));
            }
        }
    }
}