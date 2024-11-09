using EduFlex.Domain.Entities;
using EduFlex.Repository.FluentAPIs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ServicePackage> ServicePackages { get; set; }
		public DbSet<Payment> Payments { get; set; }
        public DbSet<UserPackage> UserPackages { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<ArticleImage> ArticleImages { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<CurriculumFramework> CurriculumFrameworks { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<SubjectInCurriculum> SubjectInCurriculums { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<QuestionBank> QuestionBanks { get; set; }
        public DbSet<Answer> Answers { get; set; }
		public DbSet<PlanbookCollection> PlanbookCollections { get; set; }
        public DbSet<Planbook> Planbooks { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<TeachingSchedule> TeachingSchedules { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<PlanBookShare> PlanbooksShares { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<QuestionInExam> QuestionInExams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
			modelBuilder.ApplyConfiguration(new ServicePackageConfiguration());
			modelBuilder.ApplyConfiguration(new PaymentConfiguration());
			modelBuilder.ApplyConfiguration(new UserPackageConfiguration());
			modelBuilder.ApplyConfiguration(new ArticleConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleImageConfiguration());
			modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new CurriculumFrameworkConfiguration());
            modelBuilder.ApplyConfiguration(new GradeConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectInCurriculumConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterConfiguration());
            modelBuilder.ApplyConfiguration(new LessonConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionBankConfiguration());
			modelBuilder.ApplyConfiguration(new AnswerConfiguration());
			modelBuilder.ApplyConfiguration(new PlanbookCollectionConfiguration());
            modelBuilder.ApplyConfiguration(new PlanbookConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityConfiguration());
            modelBuilder.ApplyConfiguration(new TeachingScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
            modelBuilder.ApplyConfiguration(new PlanBookShareConfiguration());
            modelBuilder.ApplyConfiguration(new ExamConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionInExamConfiguration());
        }
    }
}
