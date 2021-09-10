using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings
{
    public class PagingModel<T> where T : class
    {
        public int TotalRow { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}