using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using RepidShare.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using RepidShare.Utility;
using RepidShare.Entities;
using System.Net.Http;
using System.Net;
using System.Web.Security;
using System.Web.Routing;

namespace RepidShare.Web.Filters
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
                FormsAuthentication.SignOut();
                filterContext.Result =
               new RedirectToRouteResult(new RouteValueDictionary   
            {  
             { "action", "login" },  
            { "controller", "user" },  
            { "returnUrl", filterContext.HttpContext.Request.RawUrl}  
             });

                return;
                //redirectToUnauthorize(filterContext, "login", "user");
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

    public class BooleanRequiredAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            if (value is bool)
                return (bool)value;
            else
                return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "booleanrequired"
            };
        }
    }

    public class LayOutDataAttribute : ActionFilterAttribute
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HomeLayOutModel objHomeLayOutModel = new HomeLayOutModel();
            if (filterContext.HttpContext.Session["LayOutData"] != null)
            {
                objHomeLayOutModel = (HomeLayOutModel)filterContext.HttpContext.Session["LayOutData"];
                
            }
            else
            {
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetLayOutData");
                objHomeLayOutModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeLayOutModel>().Result : null;
                filterContext.HttpContext.Session["LayOutData"] = objHomeLayOutModel;
            }
            base.OnActionExecuting(filterContext);
        }


    }

}

