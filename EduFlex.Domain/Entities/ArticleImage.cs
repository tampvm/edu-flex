using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class ArticleImage
    {
        // Primary Key
        public string ArticleId { get; set; }
        public string ImageId { get; set; }

        // Navigation properties
        public virtual Article Article { get; set; }
        public virtual Image Image { get; set; }
    }
}
