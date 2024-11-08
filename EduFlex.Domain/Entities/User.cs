using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class User : BaseEntity
    {
        // Primary key
        public string Id { get; set; }

        // Attributes
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Gender { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? GoogleEmail { get; set; }
        public string? FacebookEmail { get; set; }
        public string? AddressText { get; set; }
        public string? City { get; set; }    
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? SchoolName { get; set; }
        public string? Teach { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool Status { get; set; }

        // Foreign keys
        public int RoleId { get; set; }
        public string? AvatarId { get; set; }
        public string? BackgroundId { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; }
        public virtual Image Avatar { get; set; }
        public virtual Image Background { get; set; }
    }
}
