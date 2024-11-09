using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class ArticleCategory
    {
        // Primary Key
        public string ArticleId { get; set; }
        public string CategoryId { get; set; }

        // Navigation properties
        public virtual Article Article { get; set; }
        public virtual Category Category { get; set; }
    }
}
