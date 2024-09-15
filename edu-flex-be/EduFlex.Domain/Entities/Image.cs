using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    [Table("Image")]
    public class Image : BaseEntity
    {
        // Primary Key
        [Key]
        public string Id { get; set; }

        // Attributes
        public string ImageUrl { get; set; }
        public string Type { get; set; }

        // Navigation properties
        public virtual ICollection<User> UserAvatars { get; set; } = new List<User>();
        public virtual ICollection<User> UserBackgrounds { get; set; } = new List<User>();
    }
}
