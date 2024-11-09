using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using Firebase.Storage;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Utils
{
    public class FirebaseService : IFirebaseService
    {
        private readonly AppConfiguration _firebaseSettings;
        private readonly FirebaseStorage _firebaseStorage;

        public FirebaseService(AppConfiguration firebaseSettings)
        {
            _firebaseSettings = firebaseSettings;

            var credential = GoogleCredential.FromFile(_firebaseSettings.Firebase.CredentialsPath);
            var storageClient = StorageClient.Create(credential);

            _firebaseStorage = new FirebaseStorage(_firebaseSettings.Firebase.StorageBucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(_firebaseSettings.Firebase.CredentialsPath),
                ThrowOnCancel = true
            });
        }

        public async Task<string> UploadAvatarImageAsync(Stream stream, string fileName)
        {
            // Upload ảnh lên Firebase Storage
            var imageUrl = await _firebaseStorage.Child("avatars").Child(fileName).PutAsync(stream);

            // Trả về URL để truy cập ảnh
            return imageUrl;
        }

        public async Task<string> UploadArticleImageAsync(Stream stream, string fileName)
        {
            // Upload ảnh lên Firebase Storage
            var imageUrl = await _firebaseStorage.Child("articles").Child(fileName).PutAsync(stream);

            // Trả về URL để truy cập ảnh
            return imageUrl;
        }
    }
}
