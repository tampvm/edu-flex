using AutoMapper;
using EduFlex.Repository.Data;
using EduFlex.Repository.Interfaces;
using EduFlex.Repository.Repositories;
using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Mappers;
using EduFlex.Service.Services;
using EduFlex.Service.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EduFlex.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services, JWTSettings jwt)
        {
            // Add JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.JWTSecretKey))
                };
            });

            // Add Swagger 
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:5173", "https://elepla.vercel.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Add services to the container.
            services.AddHttpContextAccessor();
            services.AddTransient<SeedData>();
            services.AddControllers();
            services.AddEndpointsApiExplorer(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddMemoryCache();
            services.AddHealthChecks();

            return services;
        }

        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string[] databaseConnection)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITimeService, TimeService>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IGoogleService, GoogleService>();
            services.AddScoped<IFacebookService, FacebookService>();
            services.AddScoped<ISmsSender, TwilioSmsSender>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IUrlService, UrlService>();
            services.AddScoped<IOpenAIService, OpenAIService>();

            // User
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();

            // Role
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            // Image
            services.AddScoped<IImageRepository, ImageRepository>();

            // Category
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            // Article
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleService, ArticleService>();

            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            services.AddScoped<IArticleImageRepository, ArticleImageRepository>();

            // Service Package
            services.AddScoped<IServicePackageRepository, ServicePackageRepository>();
            services.AddScoped<IServicePackageService, ServicePackageService>();

            // User Package
            services.AddScoped<IUserPackageRepository, UserPackageRepository>();
            services.AddScoped<IUserPackageService, UserPackageService>();

            // Payment
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();

            // Question Bank
            services.AddScoped<IQuestionBankRepository, QuestionBankRepository>();
            services.AddScoped<IQuestionBankService, QuestionBankService>();

			// Answer
			services.AddScoped<IAnswerRepository, AnswerRepository>();

			// Subject
			services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ISubjectService, SubjectService>();

            // Curriculum Framework
            services.AddScoped<ICurriculumFrameworkRepository, CurriculumFrameworkRepository>();
            services.AddScoped<ICurriculumFrameworkService, CurriculumFrameworkService>();

            // Grade
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IGradeService, GradeService>();

            // Subject In Curriculum
            services.AddScoped<ISubjectInCurriculumRepository, SubjectInCurriculumRepository>();
            services.AddScoped<ISubjectInCurriculumService, SubjectInCurriculumService>();

            // Chapter
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<IChapterService, ChapterService>();

            // Lesson
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ILessonService, LessonService>();

            // Planbook Collection
            services.AddScoped<IPlanbookCollectionRepository, PlanbookCollectionRepository>();
            services.AddScoped<IPlanbookCollectionService, PlanbookCollectionService>();

            // Planbook
            services.AddScoped<IPlanbookRepository, PlanbookRepository>();
            services.AddScoped<IPlanbookService, PlanbookService>();

            // Activity
            services.AddScoped<IActivityRepository, ActivityRepository>();
			services.AddScoped<IActivityService, ActivityService>();

			// Planbook Share
            services.AddScoped<IPlanBookShareRepository, PlanBookShareRepository>();

			// Teaching Schedule
			services.AddScoped<ITeachingScheduleRepository, TeachingScheduleRepository>();
            services.AddScoped<ITeachingScheduleService, TeachingScheduleService>();

            // Feedback
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IFeedbackService, FeedbackService>();

            // Exam
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IExamService, ExamService>();

            // Question In Exam
            services.AddScoped<IQuestionInExamRepository, QuestionInExamRepository>();
            services.AddScoped<IQuestionInExamService, QuestionInExamService>();

            // Add database context
            string selectedConnectionString = null;

            foreach (var connectionString in databaseConnection)
            {
                try
                {
                    // Thử kết nối với chuỗi hiện tại
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                    }

                    // Nếu kết nối thành công, sử dụng chuỗi này
                    selectedConnectionString = connectionString;
                    break;
                }
                catch
                {
                    // Nếu kết nối không thành công, tiếp tục thử chuỗi tiếp theo
                    continue;
                }
            }

            if (string.IsNullOrEmpty(selectedConnectionString))
            {
                throw new Exception("Không thể kết nối với bất kỳ cơ sở dữ liệu nào.");
            }

            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(selectedConnectionString));

            // Add AutoMapper
            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);

            return services;
        }
    }
}
