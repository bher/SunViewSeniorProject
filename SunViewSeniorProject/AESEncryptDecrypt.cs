//"Header file" containing AES functions

using System;
using System.IO; //Add this to be able to use ReadStream/WriteStream
using System.Text; //Add this to use Encoding
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography; //Add this to be able to use AES encryption
using System.Web;

namespace SunViewLogin
{
    public class AESEncryptDecrypt
    {

        /*
         * Takes in the encrypted username and password
         * Decrypts the username and password using the above function
         * Opens up the file containing the list of authorized users
         * Reads from the list.
         * If the decrypted username and password is on the list of authorized users, return True.
         */
        public static string DecryptStringAES(string cipherUser, string cipherPassword)
        {
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");
            string data;
            char[] delimit = { '\t' };
            string[] credentials;


            //Opens file containing authorized users reads them (Might update to a database rather than text file if needed)
            using (StreamReader streamfile = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(".") + "/App_Data/user.txt"))
            {
                if ((data = streamfile.ReadLine()) == null)
                    return "false";
                else
                {
                    credentials = data.Split(delimit);
                }
            }

            
            //IF statement to see if inputted username and password matches the ones stored in the file.
            if (cipherUser.Equals(credentials[0]) && cipherPassword.Equals(credentials[1]))
                return "true";
            else
                return "false";
        }

    }
}
