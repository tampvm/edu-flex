using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Domain.Enums;
using EduFlex.Repository.Common;
using EduFlex.Service.Models.ViewModels.AccountViewModels;
using EduFlex.Service.Models.ViewModels.ActivityViewModels;
using EduFlex.Service.Models.ViewModels.AnswerViewModels;
using EduFlex.Service.Models.ViewModels.ArticleViewModels;
using EduFlex.Service.Models.ViewModels.AuthViewModels;
using EduFlex.Service.Models.ViewModels.CategoryViewModels;
using EduFlex.Service.Models.ViewModels.ChapterViewModels;
using EduFlex.Service.Models.ViewModels.CurriculumViewModels;
using EduFlex.Service.Models.ViewModels.FeedbackViewModels;
using EduFlex.Service.Models.ViewModels.GradeViewModels;
using EduFlex.Service.Models.ViewModels.LessonViewModels;
using EduFlex.Service.Models.ViewModels.PaymentViewModels;
using EduFlex.Service.Models.ViewModels.PlanbookCollectionViewModels;
using EduFlex.Service.Models.ViewModels.PlanbookViewModels;
using EduFlex.Service.Models.ViewModels.QuestionBankViewModels;
using EduFlex.Service.Models.ViewModels.RoleViewModels;
using EduFlex.Service.Models.ViewModels.ServicePackageViewModels;
using EduFlex.Service.Models.ViewModels.SubjectInCurriculumViewModels;
using EduFlex.Service.Models.ViewModels.SubjectViewModels;
using EduFlex.Service.Models.ViewModels.TeachingScheduleModels;
using EduFlex.Service.Models.ViewModels.UserPackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Mappers
{
	public class MapperConfigurationsProfile : Profile
	{
		public MapperConfigurationsProfile()
		{
			CreateMap(typeof(Pagination<>), typeof(Pagination<>));

			#region User
			CreateMap<RegisterDTO, User>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => GenderEnums.Unknown.ToString()))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();

			CreateMap<SocialLoginDTO, User>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => GenderEnums.Unknown.ToString()))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();

			CreateMap<User, ViewUserProfileDTO>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.GoogleEmail, opt => opt.MapFrom(src => src.GoogleEmail))
				.ForMember(dest => dest.FacebookEmail, opt => opt.MapFrom(src => src.FacebookEmail))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
				.ForMember(dest => dest.Address, opt => opt.MapFrom(src => string.Join(", ", new[] { src.AddressLine, src.Ward, src.District, src.City }.Where(s => !string.IsNullOrWhiteSpace(s)))))
				.ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolName))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

			CreateMap<UpdateUserProfileDTO, User>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
				.ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.AddressLine))
				.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
				.ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
				.ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Ward))
				.ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolName))
				.ForMember(dest => dest.Teach, opt => opt.MapFrom(src => src.Teach)).ReverseMap();

			CreateMap<User, ViewListUserDTO>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
				.ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name))
				.ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar.ImageUrl))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

			CreateMap<CreateUserByAdminDTO, User>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();

			CreateMap<UpdateUserByAdminDTO, User>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)).ReverseMap();
			#endregion

			#region Role
			CreateMap<Role, ViewListRoleDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

			CreateMap<CreateRoleDTO, Role>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
				.ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => false)).ReverseMap();

			CreateMap<UpdateRoleDTO, Role>()
				.ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName)).ReverseMap();
			#endregion

			#region Category
			CreateMap<Category, ViewListCategoryDTO>()
				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<CreateCategoryDTO, Category>()
				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
				.ReverseMap();

			CreateMap<UpdateCategoryDTO, Category>()
				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				//.ForMember(dest=> dest.Url, opt => opt.MapFrom(src => src.Url))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ReverseMap();
			#endregion

			#region Article
			CreateMap<Article, ViewListArticleDTO>()
				.ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
				.ForMember(dest => dest.Excerpt, opt => opt.MapFrom(src => src.Excerpt))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<Article, ViewDetailArticleDTO>()
				.ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<CreateArticleDTO, Article>()
				.ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
				.ReverseMap();

			CreateMap<UpdateArticleDTO, Article>()
				.ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ReverseMap();
			#endregion

			#region QuestionBank
			CreateMap<QuestionBank, ViewListQuestionBankDTO>()
				.ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
				.ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
				.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
				.ForMember(dest => dest.Plum, opt => opt.MapFrom(src => src.Plum))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<QuestionBank, ViewDetailQuestionDTO>()
				.ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
				.ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
				.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
				.ForMember(dest => dest.Plum, opt => opt.MapFrom(src => src.Plum))
				.ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
				.ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
				.ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<CreateQuestionDTO, QuestionBank>()
				.ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
				.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
				.ForMember(dest => dest.Plum, opt => opt.MapFrom(src => src.Plum))
				.ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
				.ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
				.ReverseMap();

			CreateMap<UpdateQuestionDTO, QuestionBank>()
				.ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
				.ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
				.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
				.ForMember(dest => dest.Plum, opt => opt.MapFrom(src => src.Plum))
				.ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
				.ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
				.ReverseMap();
			#endregion

			#region Answer
			CreateMap<Answer, ViewListAnswerDTO>()
				.ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
				.ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
				.ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
				.ReverseMap();

			CreateMap<CreateAnswerDTO, Answer>()
				.ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
				.ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
				.ReverseMap();

			CreateMap<UpdateAnswerDTO, Answer>()
				.ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
				.ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
				.ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
				.ReverseMap();
			#endregion

			#region ServicePackage
			// Mapping ServicePackage to ViewServicePackageDTO
			CreateMap<ServicePackage, ViewServicePackageDTO>()
				.ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.PackageId))
				.ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
				.ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
				//.ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
				.ForMember(dest => dest.MaxLessonPlans, opt => opt.MapFrom(src => src.MaxPlanbooks))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			// Mapping CreateServicePackageDTO to ServicePackage
			CreateMap<CreateServicePackageDTO, ServicePackage>()
				.ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
				.ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
				//.ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
				.ForMember(dest => dest.MaxPlanbooks, opt => opt.MapFrom(src => src.MaxLessonPlans))
				.ReverseMap();

			// Mapping UpdateServicePackageDTO to ServicePackage
			CreateMap<UpdateServicePackageDTO, ServicePackage>()
				.ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.PackageId))
				.ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
				.ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
				//.ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
				.ForMember(dest => dest.MaxPlanbooks, opt => opt.MapFrom(src => src.MaxLessonPlans))
				.ReverseMap();
			#endregion

			#region Payment
			// Mapping Payment to PaymentDTO
			CreateMap<Payment, PaymentDTO>()
				.ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
				.ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
				//.ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.PackageName)) // Package name from associated ServicePackage
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ReverseMap();

			// Mapping Payment to PaymentDetailDTO
			CreateMap<Payment, PaymentDetailDTO>()
				.ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
				.ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
				//.ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.PackageName))  // Package name from associated ServicePackage
				//.ForMember(dest => dest.PackageDescription, opt => opt.MapFrom(src => src.Package.Description))  // Package description from associated ServicePackage
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ReverseMap();
			#endregion

			#region Subject
			CreateMap<Subject, ViewListSubjectDTO>()
			   .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
			   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			   .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			   .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
			   .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
			   .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
			   .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
			   .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
			   .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
			   .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

			CreateMap<CreateSubjectDTO, Subject>()
				.ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();

			CreateMap<UpdateSubjectDTO, Subject>()
				.ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();

            CreateMap<Subject, ViewListSuggestedSubjectDTO>()
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

            CreateMap<CreateSuggestedSubjectDTO, Subject>()
				.ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();
            #endregion

            #region Curriculum
            CreateMap<CurriculumFramework, ViewListCurriculumDTO>()
				.ForMember(dest => dest.CurriculumId, opt => opt.MapFrom(src => src.CurriculumId))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

			CreateMap<CreateCurriculumDTO, CurriculumFramework>()
				.ForMember(dest => dest.CurriculumId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();

			CreateMap<UpdateCurriculumDTO, CurriculumFramework>()
				.ForMember(dest => dest.CurriculumId, opt => opt.MapFrom(src => src.CurriculumId))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();

            CreateMap<CurriculumFramework, ViewListSuggestedCurriculumDTO>()
                .ForMember(dest => dest.CurriculumId, opt => opt.MapFrom(src => src.CurriculumId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

            CreateMap<CreateSuggestedCurriculumDTO, CurriculumFramework>()
                .ForMember(dest => dest.CurriculumId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();
            #endregion

            #region Grade
            CreateMap<Grade, ViewListGradeDTO>()
				.ForMember(dest => dest.GradeId, opt => opt.MapFrom(src => src.GradeId))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

			CreateMap<CreateGradeDTO, Grade>()
				.ForMember(dest => dest.GradeId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();

			CreateMap<UpdateGradeDTO, Grade>()
				.ForMember(dest => dest.GradeId, opt => opt.MapFrom(src => src.GradeId))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();
			#endregion

			#region TeachingSchedule
			// Mapping TeachingSchedule to ViewTeachingScheduleDTO
			CreateMap<TeachingSchedule, ViewTeachingScheduleDTO>()
				.ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
				.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
				.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
				.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
				.ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.FirstName + " " + src.Teacher.LastName))
				.ForMember(dest => dest.PlanbookTitle, opt => opt.MapFrom(src => src.Planbook.Title))
				.ReverseMap();

			// Mapping CreateTeachingScheduleDTO to TeachingSchedule
			CreateMap<CreateTeachingScheduleDTO, TeachingSchedule>()
				.ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
				.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
				.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
				.ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
				.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
				.ReverseMap();

			// Mapping UpdateTeachingScheduleDTO to TeachingSchedule
			CreateMap<UpdateTeachingScheduleDTO, TeachingSchedule>()
				.ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
				.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
				.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
				.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
				.ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
				.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
				.ReverseMap();
			#endregion

			#region Planbook
			CreateMap<Planbook, ViewListPlanbookDTO>()
				.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolName))
				.ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.TeacherName))
				.ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
				.ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName))
				//.ForMember(dest => dest.DurationInPeriods, opt => opt.MapFrom(src => src.DurationInPeriods))
				//.ForMember(dest => dest.KnowledgeObjective, opt => opt.MapFrom(src => src.KnowledgeObjective))
				//.ForMember(dest => dest.SkillsObjective, opt => opt.MapFrom(src => src.SkillsObjective))
				//.ForMember(dest => dest.QualitiesObjective, opt => opt.MapFrom(src => src.QualitiesObjective))
				//.ForMember(dest => dest.TeachingTools, opt => opt.MapFrom(src => src.TeachingTools))
				//.ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
				.ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.CollectionId))
				.ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))

				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<Planbook, ViewDetailPlanbookDTO>()
				.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolName))
				.ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.TeacherName))
				.ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
				.ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.DurationInPeriods, opt => opt.MapFrom(src => src.DurationInPeriods))
				.ForMember(dest => dest.KnowledgeObjective, opt => opt.MapFrom(src => src.KnowledgeObjective))
				.ForMember(dest => dest.SkillsObjective, opt => opt.MapFrom(src => src.SkillsObjective))
				.ForMember(dest => dest.QualitiesObjective, opt => opt.MapFrom(src => src.QualitiesObjective))
				.ForMember(dest => dest.TeachingTools, opt => opt.MapFrom(src => src.TeachingTools))
				.ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
				.ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.CollectionId))
				.ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))

				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<Planbook, ViewDetailsPlanbookDTO>()
				.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolName))
				.ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.TeacherName))
				.ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
				.ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.DurationInPeriods, opt => opt.MapFrom(src => src.DurationInPeriods + " tiết"))
				.ForMember(dest => dest.KnowledgeObjective, opt => opt.MapFrom(src => src.KnowledgeObjective))
				.ForMember(dest => dest.SkillsObjective, opt => opt.MapFrom(src => src.SkillsObjective))
				.ForMember(dest => dest.QualitiesObjective, opt => opt.MapFrom(src => src.QualitiesObjective))
				.ForMember(dest => dest.TeachingTools, opt => opt.MapFrom(src => src.TeachingTools))
				.ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
				.ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
				.ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.CollectionId))
				.ForMember(dest => dest.CollectionName, opt => opt.MapFrom(src => src.PlanbookCollection.CollectionName))
				.ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
				.ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.Lesson.Name))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<CreatePlanbookDTO, Planbook>()
				.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolName))
				.ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.TeacherName))
				.ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
				.ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.DurationInPeriods, opt => opt.MapFrom(src => src.DurationInPeriods))
				.ForMember(dest => dest.KnowledgeObjective, opt => opt.MapFrom(src => src.KnowledgeObjective))
				.ForMember(dest => dest.SkillsObjective, opt => opt.MapFrom(src => src.SkillsObjective))
				.ForMember(dest => dest.QualitiesObjective, opt => opt.MapFrom(src => src.QualitiesObjective))
				.ForMember(dest => dest.TeachingTools, opt => opt.MapFrom(src => src.TeachingTools))
				.ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
                .ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.CollectionId))
				.ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
				.ForMember(dest => dest.Activities, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
				.ReverseMap();

			CreateMap<UpdatePlanbookDTO, Planbook>()
				.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolName))
				.ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.TeacherName))
				.ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
				.ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName))
				.ForMember(dest => dest.DurationInPeriods, opt => opt.MapFrom(src => src.DurationInPeriods))
				.ForMember(dest => dest.KnowledgeObjective, opt => opt.MapFrom(src => src.KnowledgeObjective))
				.ForMember(dest => dest.SkillsObjective, opt => opt.MapFrom(src => src.SkillsObjective))
				.ForMember(dest => dest.QualitiesObjective, opt => opt.MapFrom(src => src.QualitiesObjective))
				.ForMember(dest => dest.TeachingTools, opt => opt.MapFrom(src => src.TeachingTools))
				.ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
				//.ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.CollectionId))
				//.ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
				.ForMember(dest => dest.Activities, opt => opt.Ignore())
				.ReverseMap();
			#endregion

			#region PlanbookActivity
			CreateMap<Activity, ViewListActivityDTO>()
				.ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Objective, opt => opt.MapFrom(src => src.Objective))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
				.ForMember(dest => dest.Implementation, opt => opt.MapFrom(src => src.Implementation))
				.ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.Index))
				.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
				.ReverseMap();

			CreateMap<CreateActivityDTO, Activity>()
				.ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Objective, opt => opt.MapFrom(src => src.Objective))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
				.ForMember(dest => dest.Implementation, opt => opt.MapFrom(src => src.Implementation))
				.ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.Index))
				.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
				.ReverseMap();

			CreateMap<CreateActivityForPlanbookDTO, Activity>()
				.ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Objective, opt => opt.MapFrom(src => src.Objective))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
				.ForMember(dest => dest.Implementation, opt => opt.MapFrom(src => src.Implementation))
				//.ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.Index))
				.ReverseMap();

			CreateMap<UpdateActivityForPlanbookDTO, Activity>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Objective, opt => opt.MapFrom(src => src.Objective))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Implementation, opt => opt.MapFrom(src => src.Implementation))
                //.ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.Index))
                .ReverseMap();

            CreateMap<UpdateActivityDTO, Activity>()
				.ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Objective, opt => opt.MapFrom(src => src.Objective))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
				.ForMember(dest => dest.Implementation, opt => opt.MapFrom(src => src.Implementation))
				.ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.Index))
				//.ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
				.ReverseMap();
			#endregion

			#region PlanbookCollection
			CreateMap<PlanbookCollection, ViewListPlanbookCollectionDTO>()
				.ForMember(dest => dest.CollectionId, otp => otp.MapFrom(src => src.CollectionId))
				.ForMember(dest => dest.CollectionName, otp => otp.MapFrom(src => src.CollectionName))
				.ForMember(dest => dest.IsSaved, otp => otp.MapFrom(src => src.IsSaved))
				.ForMember(dest => dest.TeacherId, otp => otp.MapFrom(src => src.TeacherId))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<PlanbookCollection, ViewDetailPlanbookCollectionDTO>()
				.ForMember(dest => dest.CollectionId, otp => otp.MapFrom(src => src.CollectionId))
				.ForMember(dest => dest.CollectionName, otp => otp.MapFrom(src => src.CollectionName))
				.ForMember(dest => dest.IsSaved, otp => otp.MapFrom(src => src.IsSaved))
				.ForMember(dest => dest.TeacherId, otp => otp.MapFrom(src => src.TeacherId))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
				.ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
				.ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
				.ReverseMap();

			CreateMap<CreatePlanbookCollectionDTO, PlanbookCollection>()
				.ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.CollectionName, opt => opt.MapFrom(src => src.CollectionName))
				.ForMember(dest => dest.IsSaved, opt => opt.MapFrom(src => src.IsSaved))
				.ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
				.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
				.ReverseMap();

			CreateMap<UpdatePlanbookCollectionDTO, PlanbookCollection>()
				.ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.CollectionId))
				.ForMember(dest => dest.CollectionName, opt => opt.MapFrom(src => src.CollectionName))
				//.ForMember(dest => dest.IsSaved, opt => opt.MapFrom(src => src.IsSaved))
				.ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
				.ReverseMap();

			CreateMap<SavePlanbookDTO, PlanbookCollection>()
				.ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.CollectionId))
				.ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
				.ForMember(dest => dest.IsSaved, opt => opt.MapFrom(src => true))
				.ReverseMap();
			#endregion

			#region SubjectInCurriculum
			CreateMap<SubjectInCurriculum, ViewListSubjectInCurriculumDTO>()
                .ForMember(dest => dest.SubjectInCurriculumId, opt => opt.MapFrom(src => src.SubjectInCurriculumId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.Join(" ", src.Subject.Name, src.Grade.Name, src.Curriculum.Name)))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Name))
                .ForMember(dest => dest.Curriculum, opt => opt.MapFrom(src => src.Curriculum.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Chapters, opt => opt.MapFrom(src => src.Chapters.Select(c => c.Name).ToList()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

            CreateMap<CreateSubjectInCurriculumDTO, SubjectInCurriculum>()
                .ForMember(dest => dest.SubjectInCurriculumId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.GradeId, opt => opt.MapFrom(src => src.GradeId))
                .ForMember(dest => dest.CurriculumId, opt => opt.MapFrom(src => src.CurriculumId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();

            CreateMap<UpdateSubjectInCurriculumDTO, SubjectInCurriculum>()
                .ForMember(dest => dest.SubjectInCurriculumId, opt => opt.MapFrom(src => src.SubjectInCurriculumId))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.GradeId, opt => opt.MapFrom(src => src.GradeId))
                .ForMember(dest => dest.CurriculumId, opt => opt.MapFrom(src => src.CurriculumId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();
            #endregion

            #region Chapter
			CreateMap<Chapter, ViewListChapterDTO>()
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SubjectInCurriculum, opt => opt.MapFrom(src => src.SubjectInCurriculum.CurriculumId))
				.ForMember(dest => dest.Lessons, opt => opt.MapFrom(src => src.Lessons.Select(l => l.Name).ToList()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

            CreateMap<CreateChapterDTO, Chapter>()
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SubjectInCurriculumId, opt => opt.MapFrom(src => src.SubjectInCurriculumId)).ReverseMap();

            CreateMap<UpdateChapterDTO, Chapter>()
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SubjectInCurriculumId, opt => opt.MapFrom(src => src.SubjectInCurriculumId)).ReverseMap();
            #endregion

            #region Lesson
            CreateMap<Lesson, ViewListLessonDTO>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Objectives, opt => opt.MapFrom(src => src.Objectives))
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(dest => dest.ChapterName, opt => opt.MapFrom(src => src.Chapter.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)).ReverseMap();

            CreateMap<CreateLessonDTO, Lesson>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Objectives, opt => opt.MapFrom(src => src.Objectives))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId)).ReverseMap();

            CreateMap<UpdateLessonDTO, Lesson>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Objectives, opt => opt.MapFrom(src => src.Objectives))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId)).ReverseMap();
            #endregion

            #region Feedback
            // Mapping Feedback to ViewFeedbackDTO
            CreateMap<Feedback, ViewFeedbackDTO>()
                .ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => src.FeedbackId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.FirstName + " " + src.Teacher.LastName))
                .ForMember(dest => dest.PlanbookTitle, opt => opt.MapFrom(src => src.Planbook.Title))
                .ReverseMap();

            // Mapping CreateFeedbackDTO to Feedback
            CreateMap<CreateFeedbackDTO, Feedback>()
                .ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
                .ForMember(dest => dest.PlanbookId, opt => opt.MapFrom(src => src.PlanbookId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();
            #endregion

            #region UserPackage
            CreateMap<UserPackage, ViewUserPackageDTO>()
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.PackageName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Package.Price))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Package.Discount))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => DateTime.UtcNow > src.EndDate))
                .ReverseMap();

            CreateMap<UserPackage, ViewListUserPackageDTO>()
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.PackageName))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => DateTime.UtcNow > src.EndDate))
                .ReverseMap();
            #endregion
        }
    }
}
