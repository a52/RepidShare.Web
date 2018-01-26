using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using RepidShare.Admin.Models;
using System.Web.Configuration;
using System.Configuration;
using System.Web;


namespace RepidShare.Admin.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }

    public class AuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string CurrentURL = string.Format("/{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName).ToLower();
            // Security Token point No 12 . 09Jan2015
            if (filterContext.HttpContext.Response.StatusCode == 403)
            {
                redirectToUnauthorize(filterContext, "login", "user");
            }

            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                redirectToUnauthorize(filterContext, "login", "user");
            }
            base.OnActionExecuting(filterContext);
        }

        void redirectToUnauthorize(ActionExecutingContext filterContext, string actionName, string controllerName)
        {
            System.Web.Routing.RouteValueDictionary route = new System.Web.Routing.RouteValueDictionary();
            route.Add("action", actionName);
            route.Add("controller", controllerName);
            filterContext.Result = new RedirectToRouteResult(route);
        }
    }
}
