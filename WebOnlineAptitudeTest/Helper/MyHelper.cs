using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebOnlineAptitudeTest
{
    public static class MyHelper
    {
        public static string ToMD5(this String s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            var hash = MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static string ToIso8601FullDate(this DateTime? d)
        {
            if (!d.HasValue) return null;

            return d.Value.ToString("yyyy-MM-dd");
        }
    }
}