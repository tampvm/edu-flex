using EduFlex.Repository.Data;
using EduFlex.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IArticleImageRepository _articleImageRepository;
        private readonly IServicePackageRepository _servicePackageRepository;
        private readonly IUserPackageRepository _userPackageRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IQuestionBankRepository _questionBankRepository;
        private readonly IAnswerRepository _answerRepository;
		private readonly ISubjectRepository _subjectRepository;
        private readonly ICurriculumFrameworkRepository _curriculumFrameworkRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ISubjectInCurriculumRepository _subjectInCurriculumRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IPlanbookCollectionRepository _planbookCollectionRepository;
        private readonly IPlanbookRepository _planbookRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IPlanBookShareRepository _planBookShareRepository;
        private readonly ITeachingScheduleRepository _teachingScheduleRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionInExamRepository _questionInExamRepository;

        public UnitOfWork(AppDbContext dbContext,
            IAccountRepository accountRepository,
            IRoleRepository roleRepository,
            IImageRepository imageRepository,
            ICategoryRepository categoryRepository,
            IArticleRepository articleRepository,
            IArticleCategoryRepository articleCategoryRepository,
            IArticleImageRepository articleImageRepository,
            IServicePackageRepository servicePackageRepository,
            IUserPackageRepository userPackageRepository,
            IPaymentRepository paymentRepository,
            IQuestionBankRepository questionBankRepository,
			IAnswerRepository answerRepository,
            ISubjectRepository subjectRepository,
            ICurriculumFrameworkRepository curriculumFrameworkRepository,
            IGradeRepository gradeRepository,
            ISubjectInCurriculumRepository subjectInCurriculumRepository,
            IChapterRepository chapterRepository,
            ILessonRepository lessonRepository,
            IPlanbookCollectionRepository planbookCollectionRepository,
            IPlanbookRepository planbookRepository,
            IActivityRepository activityRepository,
            IPlanBookShareRepository planBookShareRepository,
            ITeachingScheduleRepository teachingScheduleRepository,
            IFeedbackRepository feedbackRepository,
            IExamRepository examRepository,
            IQuestionInExamRepository questionInExamRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _imageRepository = imageRepository;
            _categoryRepository = categoryRepository;
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _articleImageRepository = articleImageRepository;
            _servicePackageRepository = servicePackageRepository;
            _userPackageRepository = userPackageRepository;
            _paymentRepository = paymentRepository;
            _questionBankRepository = questionBankRepository;
            _answerRepository = answerRepository;
            _subjectRepository = subjectRepository;
            _curriculumFrameworkRepository = curriculumFrameworkRepository;
            _gradeRepository = gradeRepository;
            _subjectInCurriculumRepository = subjectInCurriculumRepository;
            _chapterRepository = chapterRepository;
            _lessonRepository = lessonRepository;
            _planbookCollectionRepository = planbookCollectionRepository;
            _planbookRepository = planbookRepository;
            _activityRepository = activityRepository;
            _planBookShareRepository = planBookShareRepository;
            _teachingScheduleRepository = teachingScheduleRepository;
            _feedbackRepository = feedbackRepository;
            _examRepository = examRepository;
            _questionInExamRepository = questionInExamRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public IRoleRepository RoleRepository => _roleRepository;

        public IImageRepository ImageRepository => _imageRepository;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public IArticleRepository ArticleRepository => _articleRepository;

        public IArticleCategoryRepository ArticleCategoryRepository => _articleCategoryRepository;

        public IArticleImageRepository ArticleImageRepository => _articleImageRepository;

        public IServicePackageRepository ServicePackageRepository => _servicePackageRepository;

        public IUserPackageRepository UserPackageRepository => _userPackageRepository;

        public IPaymentRepository PaymentRepository => _paymentRepository;

        public IQuestionBankRepository QuestionBankRepository => _questionBankRepository;

		public IAnswerRepository AnswerRepository => _answerRepository;

		public ISubjectRepository SubjectRepository => _subjectRepository;

        public ICurriculumFrameworkRepository CurriculumFrameworkRepository => _curriculumFrameworkRepository;

        public IGradeRepository GradeRepository => _gradeRepository;

        public ISubjectInCurriculumRepository SubjectInCurriculumRepository => _subjectInCurriculumRepository;

        public IChapterRepository ChapterRepository => _chapterRepository;

        public ILessonRepository LessonRepository => _lessonRepository;

        public IPlanbookCollectionRepository PlanbookCollectionRepository => _planbookCollectionRepository;

        public IPlanbookRepository PlanbookRepository => _planbookRepository;

        public IActivityRepository ActivityRepository => _activityRepository;

		public IPlanBookShareRepository PlanBookShareRepository => _planBookShareRepository;

		public ITeachingScheduleRepository TeachingScheduleRepository => _teachingScheduleRepository;

        public IFeedbackRepository FeedbackRepository => _feedbackRepository;

        public IExamRepository ExamRepository => _examRepository;

        public IQuestionInExamRepository QuestionInExamRepository => _questionInExamRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
