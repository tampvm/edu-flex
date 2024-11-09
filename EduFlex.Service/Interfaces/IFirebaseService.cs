using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> UploadAvatarImageAsync(Stream stream, string fileName);
        Task<string> UploadArticleImageAsync(Stream stream, string fileName);
    }
}
