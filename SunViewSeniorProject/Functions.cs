using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO; //Add this to be able to use ReadStream/WriteStream

namespace SunViewLogin
{
    public class Functions
    {
        public static string Authenticate()
        {
            string auth = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/authorized.txt");

            return auth;
        }
        public static void authorizeUser(string success)
        {
            File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/authorized.txt", success);
        }
        /*
        public static void saveIP(string ipAdd)
        {
            File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/ipadd.txt", ipAdd);
        }

        public static string IPchecker(string ip)
        {
            string data;
            using (StreamReader streamfile = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/ipadd.txt"))
            {

                if ((data = streamfile.ReadLine()) != null)
                {
                    if (data.Equals(ip))
                        return "true";
                }

                return "false";
            }
        }
        */
        public static void userLoggedOut()
        {

            File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/authorized.txt", "false");
        }
    }
}