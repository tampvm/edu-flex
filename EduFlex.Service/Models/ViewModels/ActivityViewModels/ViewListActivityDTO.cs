using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.ActivityViewModels
{
	public class ViewListActivityDTO
	{
		public string ActivityId { get; set; } 
		public string Title { get; set; } // (Hoạt động 1: Xác định vấn đề, Hoạt động 2: Hình thành kiến thức mới, Hoạt động 3: Luyện tập, Hoạt động 4: Vận dụng)
        public string? Objective { get; set; } // Mục tiêu
		public string? Content { get; set; } // Nội dung
        public string? Product { get; set; } // Sản phẩm
        public string? Implementation { get; set; } // Tổ chức thực hiện
        public int Index { get; set; }
		public string PlanbookId { get; set; }
	}
}
