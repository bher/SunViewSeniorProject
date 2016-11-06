using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunViewLogin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        //The Javascript file will call these methods.
        //These methods then call the corresponding method in a different C# class file
        //Ensure that the other class files are using the same namespace as this file.
        [System.Web.Services.WebMethod]
        public static string checkUser(string user, string pass)
        {
            string success = AESEncryptDecrypt.DecryptStringAES(user,pass);

            if (success.Equals("true"))
            {
                Functions.userLoggedOut();
                String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ip))
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                else
                    ip = ip.Split(',')[0];

                Functions.saveIP(ip);
            }

            return success;
        }

        [System.Web.Services.WebMethod]
        public static string checkIP()
        {
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            else
                ip = ip.Split(',')[0];   

            return Functions.IPchecker(ip);
        }
        [System.Web.Services.WebMethod]
        public static void logout()
        {
            Functions.userLoggedOut();
        }
        [System.Web.Services.WebMethod]
        public static void logintimer()
        {
            Functions.startTimer();
        }
    }
}