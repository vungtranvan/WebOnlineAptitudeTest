using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebOnlineAptitudeTest.Models.Entities
{
    public partial class OnlineTestDbContext : DbContext
    {
        public OnlineTestDbContext()
            : base("name=OnlineTestDbContext")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<CategoryExam> CategoryExams { get; set; }
        public virtual DbSet<HistoryTest> HistoryTests { get; set; }
        public virtual DbSet<HistoryTestDetail> HistoryTestDetails { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Candidate>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Candidate>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.HistoryTests)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.Transfers)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CategoryExam>()
                .HasMany(e => e.HistoryTests)
                .WithRequired(e => e.CategoryExam)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CategoryExam>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.CategoryExam)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HistoryTest>()
                .HasMany(e => e.HistoryTestDetails)
                .WithRequired(e => e.HistoryTest)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HistoryTestDetail>()
                .Property(e => e.AnswerChoice)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.CorrectAnswer)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.HistoryTestDetails)
                .WithRequired(e => e.Question)
                .WillCascadeOnDelete(false);
        }
    }
}
