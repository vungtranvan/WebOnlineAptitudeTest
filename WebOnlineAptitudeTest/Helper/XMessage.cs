using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest
{
    public class XMessage
    {
        public string Title { get; set; }
        public string Msg { get; set; }
        public EnumCategoryMess Types { get; set; }
        public XMessage(string title,string msg, EnumCategoryMess types)
        {
            Title = title;
            Msg = msg;
            Types = types;
        }
    }

    public enum EnumCategoryMess
    {
        error,
        success,
        warning
    }
}