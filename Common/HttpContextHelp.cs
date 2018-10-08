using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class HttpContextHelp
    {
        public static string GetClientIp(HttpRequestBase request)
        {
            if (request == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrWhiteSpace(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                return request.ServerVariables["REMOTE_ADDR"];
            }
            return request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
        }

        public static string GetClientIp(HttpContext context)
        {
            if (context == null)
            {
                return string.Empty;
            }
            var req = context.Request;
            if (string.IsNullOrWhiteSpace(req.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                return req.ServerVariables["REMOTE_ADDR"];
            }
            return req.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
        }
    }
}
