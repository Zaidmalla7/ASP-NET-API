using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;

namespace Hope.Infrastructure.Base
{
    public class Security
    {
        public string EncryptString(string message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("P@ssw0rd"));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(message);

            ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);

            var d = Convert.ToBase64String(Results);

            return d;
        }

        public string DecryptString(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("P@ssw0rd"));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            string urlDecode = HttpUtility.UrlDecode(Message);
            byte[] DataToEncrypt;

            try
            {


                if (urlDecode.Length >= 12)
                {
                    DataToEncrypt = Convert.FromBase64String(Message);
                }
                else
                {
                    return Message;
                }


                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            catch (Exception)
            {

                return Message;
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }


            var d = UTF8.GetString(Results);

            return d;

        }
    }
}
