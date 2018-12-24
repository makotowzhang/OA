using System.Web;
using System.Web.Mvc;
using Model.SystemModel;
using Model.EnumModel;
using Common;
using System;

namespace OAWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class LogFilter : ActionFilterAttribute
    {
        public LogFilter(string actionName, string menuName, LogActionType logActionType, string description = "")
        {
            Log = new LogModel()
            {
                ActionName = actionName,
                MenuName = menuName,
                ActionType = logActionType.ToString(),
                Description = description
            };
        }
        public LogModel Log { get; set; }


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //base.OnActionExecuted(filterContext);
            //Log.IpAddress = HttpContextHelp.GetClientIp(filterContext.HttpContext.Request);
            //string user = filterContext.RequestContext.HttpContext.User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    Log.DoUser = Guid.Empty;
            //}
            //else
            //{
            //    Log.DoUser = Guid.Parse(user);
            //}
            //new Business.SystemBusiness.LogBusiness().AddLog(Log);
        }
    }

    public class PageAuthorizeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ActionResult notAuthoriz = new ContentResult() { Content = "页面未授权请联系管理员" };
            if (string.IsNullOrWhiteSpace(filterContext.RequestContext.HttpContext.Request["MenuId"]))
            {
                filterContext.Result = notAuthoriz;
                return;
            }
            Guid.TryParse(filterContext.RequestContext.HttpContext.Request["MenuId"], out Guid menuId);
            if (menuId == Guid.Empty)
            {
                filterContext.Result = notAuthoriz;
                return;
            }
            Guid.TryParse(filterContext.RequestContext.HttpContext.User.Identity.Name, out Guid userId);
            if (!new Business.SystemBusiness.AuthorizeBusiness().CheckUserViewAuthorize(menuId, userId))
            {
                filterContext.Result = notAuthoriz;
            }
        }
    }
}
