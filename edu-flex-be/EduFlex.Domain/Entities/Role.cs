using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // Attributes
        public string Name { get; set; }
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
