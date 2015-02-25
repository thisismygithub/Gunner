using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace CMoney.WebBackend.WebFunction
{
    public class Security
    {
        public static string GetMD5(string InputStr)
        {
            byte[] textBytes = Encoding.Default.GetBytes(InputStr);

            try
            {
                MD5CryptoServiceProvider cryptHandler = new MD5CryptoServiceProvider();

                byte[] hash = cryptHandler.ComputeHash(textBytes);

                StringBuilder sb = new StringBuilder();

                foreach (byte a in hash)
                {
                    if (a < 16)
                    {
                        sb.Append("0");
                    }

                    sb.Append(a.ToString("x"));
                }

                return sb.ToString();
            }
            catch
            {
                throw;
            }
        }
    }
}
