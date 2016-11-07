using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO; //Add this to be able to use ReadStream/WriteStream

namespace SunViewLogin
{
    public class Functions
    {
        
        public static void saveIP(string ipAdd)
        {
            using (StreamWriter streamfile = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/ipadd.txt"))
            {
                //Function to write ipAddress to txtfile
                //Store whether the user is authorized or not.
                streamfile.WriteLine(ipAdd);
            }
            //startTimer();
        }

        public static string IPchecker(string ip)
        {
            string data;
            using (StreamReader streamfile = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/ipadd.txt"))
            {
                if ((data = streamfile.ReadLine()) == null)
                    return "false";
                else
                if (data.Equals(ip))
                    return "true";
                else
                    return "false";
            }
        }

        public static void userLoggedOut()
        {
            using (StreamWriter streamfile = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/ipadd.txt"))
            {
                //Function to write ipAddress to txtfile
                //Store whether the user is authorized or not.
                streamfile.WriteLine("");
            }
        }
    }
}