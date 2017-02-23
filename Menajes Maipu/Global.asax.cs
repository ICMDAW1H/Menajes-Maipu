using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Menajes_Maipu
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
         
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        public class HttpRequestValidationExceptionAttribute : FilterAttribute, IExceptionFilter
        {

            public void OnException(ExceptionContext filterContext)
            {
                if (!filterContext.ExceptionHandled && filterContext.Exception is HttpRequestValidationException)
                {
                    filterContext.Result = new RedirectResult("Index");
                    filterContext.ExceptionHandled = true;
                }
            }
        }
    }
}
