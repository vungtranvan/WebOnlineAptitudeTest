using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class TransferRepository : RepositoryBase<Transfer>, ITransferRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransferRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;
        }
        public PagingModel<Transfer> GetData(string keyword, int page, int pageSize)
        {
            var lstTransfer = new List<Transfer>();

            if (!string.IsNullOrEmpty(keyword))
            {
                lstTransfer = base.GetMulti(x => x.Deleted == false && (x.Candidate.Name.Contains(keyword)
                       || x.Candidate.UserName.Contains(keyword) || x.Candidate.Email.Contains(keyword)), new string[] { "Candidate"}).ToList();
            }
            else
            {
                lstTransfer = base.GetMulti(x => x.Deleted == false && (x.Candidate.Name.Contains(keyword)
                       || x.Candidate.UserName.Contains(keyword) || x.Candidate.Email.Contains(keyword)), new string[] { "Candidate"}).OrderByDescending(y => y.Id).ToList();
            }

            int totalRow = lstTransfer.Count();

            var data = lstTransfer.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<Transfer>() { TotalRow = totalRow, Items = data };
        }

        public IEnumerable<Transfer> GetTop(int id = 10)
        {
           return base.DbContext.Transfers.OrderByDescending(x => x.CreatedDate).Take(id);
        }

        public bool Locked(int id)
        {
            var cadi = base.GetSingleById(id);
            if (cadi == null)
            {
                return false;
            }
            cadi.Deleted = !cadi.Deleted;
            _unitOfWork.Commit();
            return true;
        }
    }
}