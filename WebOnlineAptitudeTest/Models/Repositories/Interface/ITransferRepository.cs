﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories.Interface
{
    public interface ITransferRepository : IRepository<Transfer>
    {
        bool Locked(int id);
        PagingModel<Transfer> GetData(string keyword, int page, int pageSize);
    }
}