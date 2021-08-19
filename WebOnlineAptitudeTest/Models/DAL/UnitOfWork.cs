using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Models.DAL
{
    public class UnitOfWork : IDisposable
    {
        private OnlineTestDbContext context = new OnlineTestDbContext();
        private GenericRepository<Admin> adminRepository;
        private GenericRepository<Candidate> candidateRepository;
        private GenericRepository<CategoryExam> categoryExamRepository;
        private GenericRepository<HistoryTest> historyTestRepository;
        private GenericRepository<HistoryTestDetail> historyTestDetailRepository;
        private GenericRepository<Question> questionRepository;
        private GenericRepository<Answer> answerRepository;
        private GenericRepository<Transfer> transferRepository;

        public GenericRepository<Admin> AdminRepository
        {
            get
            {
                if (this.adminRepository == null)
                {
                    this.adminRepository = new GenericRepository<Admin>(context);
                }
                return adminRepository;
            }
        }

        public GenericRepository<Candidate> CandidateRepository
        {
            get
            {
                if (this.candidateRepository == null)
                {
                    this.candidateRepository = new GenericRepository<Candidate>(context);
                }
                return candidateRepository;
            }
        }

        public GenericRepository<CategoryExam> CategoryExamRepository
        {
            get
            {
                if (this.categoryExamRepository == null)
                {
                    this.categoryExamRepository = new GenericRepository<CategoryExam>(context);
                }
                return categoryExamRepository;
            }
        }

        public GenericRepository<HistoryTest> HistoryTestRepository
        {
            get
            {
                if (this.historyTestRepository == null)
                {
                    this.historyTestRepository = new GenericRepository<HistoryTest>(context);
                }
                return historyTestRepository;
            }
        }

        public GenericRepository<HistoryTestDetail> HistoryTestDetailRepository
        {
            get
            {
                if (this.historyTestDetailRepository == null)
                {
                    this.historyTestDetailRepository = new GenericRepository<HistoryTestDetail>(context);
                }
                return historyTestDetailRepository;
            }
        }

        public GenericRepository<Transfer> TransferRepository
        {
            get
            {
                if (this.transferRepository == null)
                {
                    this.transferRepository = new GenericRepository<Transfer>(context);
                }
                return transferRepository;
            }
        }

        public GenericRepository<Question> QuestionRepository
        {
            get
            {
                if (this.questionRepository == null)
                {
                    this.questionRepository = new GenericRepository<Question>(context);
                }
                return questionRepository;
            }
        }

        public GenericRepository<Answer> AnswerRepository
        {
            get
            {
                if (this.answerRepository == null)
                {
                    this.answerRepository = new GenericRepository<Answer>(context);
                }
                return answerRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}