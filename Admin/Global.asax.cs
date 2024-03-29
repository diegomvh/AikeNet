﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
        
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var cookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
            var authCookie = this.Context.Request.Cookies[cookieName];
            if (authCookie != null) {
                var authTicket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null) {
                    var groups = authTicket.UserData.Split('|');
                    var id = new System.Security.Principal.GenericIdentity(authTicket.Name, "LdapAuthentication");
                    var principal = new System.Security.Principal.GenericPrincipal(id, groups);
                    this.Context.User = principal;
                }
            }

        }

    }
}