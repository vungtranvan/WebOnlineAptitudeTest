using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Models.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private OnlineTestDbContext dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public OnlineTestDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public void Commit()
        {
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    error += "<br/>" + eve.Entry.Entity.GetType().Name +" - "+ eve.Entry.State;
                    foreach (var ve in eve.ValidationErrors)
                    {
                       error += "<br/>" + ve.PropertyName +" - "+ ve.ErrorMessage;
                    }
                }
                //error = error;
                throw;
            }
           
        }
    }
}
