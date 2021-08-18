﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest
{
    public static class MyHelper
    {
        public static string ToIso8601FullDate(this DateTime? d)
        {
            if (!d.HasValue) return null;

            return d.Value.ToString("yyyy-MM-dd");
        }
    }
}