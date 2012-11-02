using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Admin.Extensions
{
    public static class FormsAuthentication
    {
        public static void SignIn(Admin.Security.User user, bool createPersistentCookie) {
            var authTicket = new System.Web.Security.FormsAuthenticationTicket(1,
                user.UserName, DateTime.Now,
                DateTime.Now.AddMinutes(60),
                createPersistentCookie,
				user.ToJson());

            var encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, encryptedTicket);
            if (createPersistentCookie) 
                authCookie.Expires = authTicket.Expiration;
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }
        public static void SignOut() {
            System.Web.Security.FormsAuthentication.SignOut();
        }
    }
}