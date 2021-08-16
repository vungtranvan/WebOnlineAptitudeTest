using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebOnlineAptitudeTest
{
    public static class MyString
    {
        public static String ToBase64(this String s)
        {
            if (s!=null)
            {
                var bytes = Encoding.UTF8.GetBytes(s);
                return Convert.ToBase64String(bytes);
            }
            return s;
        }
        public static String ToFrom64(this String s)
        {
            if (s != null)
            {
                var bytes = Convert.FromBase64String(s);
                return Encoding.UTF8.GetString(bytes);
            }
            return s;
        }

        public static string ToMD5(this String s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            var hash = MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}