using BCrypt.Net;
using EduFlex.Domain.Entities;
using EduFlex.Domain.Enums;
using EduFlex.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Data
{
    public class SeedData
    {
        public async Task Initialize(IUnitOfWork unitOfWork)
        {
            await InitializeRole(unitOfWork);
            await InitializeAccount(unitOfWork);
            await InitializeSubject(unitOfWork);
            await InitializeCurriculum(unitOfWork);
            await InitializeGrade(unitOfWork);
        }

        private static async Task InitializeRole(IUnitOfWork unitOfWork)
        {
            var roles = Enum.GetNames(typeof(RoleEnums)); // Lấy danh sách tên các vai trò từ enum RoleEnums
            foreach (var roleName in roles)
            {
                var roleExists = await unitOfWork.RoleRepository.RoleExistsAsync(roleName); // Kiểm tra xem vai trò đã tồn tại chưa
                if (!roleExists)
                {
                    var role = new Role
                    {
                        Name = roleName,
                        IsDefault = true,
                        CreatedBy = "system",
                        IsDeleted = false
                    };

                    await unitOfWork.RoleRepository.AddAsync(role); 
                }
            }

            await unitOfWork.SaveChangeAsync(); 
        }

        private static async Task InitializeAccount(IUnitOfWork unitOfWork)
        {
            string password = "123@123";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var adminRole = await unitOfWork.RoleRepository.GetRoleByNameAsync(RoleEnums.Admin.ToString());

            if (adminRole != null)
            {
                var existingAdmin = await unitOfWork.AccountRepository.GetUserByUsernameAsync("admin");
                if (existingAdmin == null)
                {
                    var admin = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        Username = "admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true,
                        PasswordHash = hashedPassword,
                        Gender = GenderEnums.Unknown.ToString(),
                        Status = true,
                        RoleId = adminRole.RoleId,
                        CreatedBy = "system",
                    };

                    await unitOfWork.AccountRepository.AddAsync(admin);
                }
            }

            var managerRole = await unitOfWork.RoleRepository.GetRoleByNameAsync(RoleEnums.Manager.ToString());

            if (managerRole != null)
            {
                var existingManager = await unitOfWork.AccountRepository.GetUserByUsernameAsync("manager");
                if (existingManager == null)
                {
                    var manager = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        Username = "manager",
                        Email = "manager@example.com",
                        EmailConfirmed = true,
                        PasswordHash = hashedPassword,
                        Gender = GenderEnums.Unknown.ToString(),
                        Status = true,
                        RoleId = managerRole.RoleId,
                        CreatedBy = "system",
                    };

                    await unitOfWork.AccountRepository.AddAsync(manager);
                }
            }
            

            var academicStaffRole = await unitOfWork.RoleRepository.GetRoleByNameAsync(RoleEnums.AcademicStaff.ToString());

            if (academicStaffRole != null)
            {
                var existingAcademicStaff = await unitOfWork.AccountRepository.GetUserByUsernameAsync("academicstaff");
                if (existingAcademicStaff == null)
                {
                    var academicStaff = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        Username = "academicstaff",
                        Email = "academicstaff@example.com",
                        EmailConfirmed = true,
                        PasswordHash = hashedPassword,
                        Gender = GenderEnums.Unknown.ToString(),
                        Status = true,
                        RoleId = academicStaffRole.RoleId,
                        CreatedBy = "system",
                    };

                    await unitOfWork.AccountRepository.AddAsync(academicStaff);
                }
            }

            var teacherRole = await unitOfWork.RoleRepository.GetRoleByNameAsync(RoleEnums.Teacher.ToString());
            
            if (teacherRole != null)
            {
                var existingTeacher = await unitOfWork.AccountRepository.GetUserByUsernameAsync("teacher");
                if (existingTeacher == null)
                {
                    var teacher = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        Username = "teacher",
                        Email = "teacher@example.com",
                        EmailConfirmed = true,
                        PasswordHash = hashedPassword,
                        Gender = GenderEnums.Unknown.ToString(),
                        Status = true,
                        RoleId = teacherRole.RoleId,
                        CreatedBy = "system",
                    };

                    await unitOfWork.AccountRepository.AddAsync(teacher);
                }
            }

            await unitOfWork.SaveChangeAsync();
        }

        private static async Task InitializeSubject(IUnitOfWork unitOfWork)
        {
            var subjects = new List<string> { "Toán", "Ngữ Văn", "Tiếng Anh", "Vật Lý", "Hóa Học", "Sinh Học", "Lịch Sử", "Địa Lý", "Tin Học", "Công Nghệ", "Giáo Dục Công Dân" };

            foreach (var subjectName in subjects)
            {
                var subjectExists = await unitOfWork.SubjectRepository.SubjectExistsAsync(subjectName);
                if (subjectExists is null)
                {
                    var subject = new Subject
                    {
                        SubjectId = Guid.NewGuid().ToString(),
                        Name = subjectName,
                        IsApproved = true,
                        CreatedBy = "system",
                        IsDeleted = false
                    };

                    await unitOfWork.SubjectRepository.AddAsync(subject);
                }
            }

            await unitOfWork.SaveChangeAsync();
        }

        private static async Task InitializeCurriculum(IUnitOfWork unitOfWork)
        {
            var curriculums = new List<string> { "Kết nối tri thức", "Chân trời sáng tạo", "Cánh diều" };

            foreach (var curriculumName in curriculums)
            {
                var curriculumExists = await unitOfWork.CurriculumFrameworkRepository.CurriculumFrameworkExistsAsync(curriculumName);
                if (curriculumExists is null)
                {
                    var curriculum = new CurriculumFramework
                    {
                        CurriculumId = Guid.NewGuid().ToString(),
                        Name = curriculumName,
                        IsApproved = true,
                        CreatedBy = "system",
                        IsDeleted = false
                    };

                    await unitOfWork.CurriculumFrameworkRepository.AddAsync(curriculum);
                }
            }

            await unitOfWork.SaveChangeAsync();
        }

        private static async Task InitializeGrade(IUnitOfWork unitOfWork)
        {
            var grades = new List<string> { "Lớp 10", "Lớp 11", "Lớp 12" };

            foreach (var gradeName in grades)
            {
                var gradeExists = await unitOfWork.GradeRepository.GradeExistsAsync(gradeName);
                if (!gradeExists)
                {
                    var grade = new Grade
                    {
                        GradeId = Guid.NewGuid().ToString(),
                        Name = gradeName,
                        CreatedBy = "system",
                        IsDeleted = false
                    };

                    await unitOfWork.GradeRepository.AddAsync(grade);
                }
            }

            await unitOfWork.SaveChangeAsync();
        }
    }

}
