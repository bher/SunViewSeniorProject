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
        /*Current Solution to checking for authorized users:
         * Add anoter function to this class.
         * What the class will do is take in the cipherText, key and iv.
         * Call the decryption algorithm 'DecryptStringFromBytes' from here, and store results
         * Open up the "database", search the database.
         * Depending on if the login credentials is correct, grant access or deny.
         */
        public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        } //End DecryptSTringFromBytes()

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }//End EncryptStringToBytes

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

            
            //var encryptUser = Convert.FromBase64String(cipherUser);
            //var encryptPass = Convert.FromBase64String(cipherPassword);

            //var deUser = DecryptStringFromBytes(encryptUser, keybytes, iv);
            //var dePass = DecryptStringFromBytes(encryptPass, keybytes, iv);

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
