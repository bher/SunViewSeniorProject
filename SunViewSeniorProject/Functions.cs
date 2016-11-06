using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO; //Add this to be able to use ReadStream/WriteStream
using System.Timers; //Add this to use timers

namespace SunViewLogin
{
    public class Functions
    {
        static Timer time;
        public static void startTimer()
        {
            time = new Timer(360000); //sets timer in miliseconds: currently set for 1 hours
            time.Elapsed += new ElapsedEventHandler(loginexpired);
            time.Enabled = true;
        }

        static void loginexpired(object sender, ElapsedEventArgs e)
        {
            time.Enabled = false;
            userLoggedOut();
        }

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

                if (data.Equals(ip))
                    return "true";
                else
                    return "false";
            }
        }

        public static void userLoggedOut()
        {
            File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/ipadd.txt", String.Empty);
        }
    }
}