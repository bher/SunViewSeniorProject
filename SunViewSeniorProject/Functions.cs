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
        public static string[] securityQuestion(string user)
        {
            string data;
            char[] delimit = { '\t' };
            string[] credentials;
            string[] incorrect = { "false" };


            //Opens file containing authorized users reads them (Might update to a database rather than text file if needed)
            using (StreamReader streamfile = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/user.txt"))
            {
                if ((data = streamfile.ReadLine()) == null)
                    return incorrect;
                else
                {
                    credentials = data.Split(delimit);
                }
            }

            if (user.Equals(credentials[0]))
            {
                using (StreamReader streamfile = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/SecurityQuestions.txt"))
                {
                    if ((data = streamfile.ReadLine()) == null)
                        return incorrect;
                    else
                    {
                        credentials = data.Split(delimit);
                    }
                }

                return credentials;
            }
            else
                return incorrect;
        }

        public static string checkAnswers(string aone, string atwo, string athree)
        {
            string data;
            char[] delimit = { '\t' };
            string[] credentials;


            //Opens file containing authorized users reads them (Might update to a database rather than text file if needed)
            using (StreamReader streamfile = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/Answers.txt"))
            {
                if ((data = streamfile.ReadLine()) == null)
                    return "false";
                else
                {
                    credentials = data.Split(delimit);
                }
            }
            if (aone.Equals(credentials[0]) && atwo.Equals(credentials[1]) && athree.Equals(credentials[2]))
            {
                return "true";
            }
            else
                return "false";

        }

        public static void changePassword(string newpass)
        {
            string data;
            char[] delimit = { '\t' };
            string[] credentials;


            //Opens file containing authorized users reads them (Might update to a database rather than text file if needed)
            using (StreamReader streamfile = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/user.txt"))
            {
                if ((data = streamfile.ReadLine()) == null)
                    return;
                else
                {
                    credentials = data.Split(delimit);
                }
            }
            data = credentials[0] + "\t" + newpass;
            File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/user.txt", data);
        }
    }
}