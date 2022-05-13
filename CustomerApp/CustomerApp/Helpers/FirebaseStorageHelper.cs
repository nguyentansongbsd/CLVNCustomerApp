using System;
using System.IO;
using System.Threading.Tasks;
using Firebase.Storage;

namespace CustomerApp.Helper
{
    public class FirebaseStorageHelper
    {
        public string likeStorage = "smsappcrm.appspot.com";
        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            FirebaseStorage firebaseStorage = new FirebaseStorage(likeStorage);
            var imageUrl = await firebaseStorage
                .Child("pdf")
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }
        public async Task<string> GetFile(string fileName)
        {
            FirebaseStorage firebaseStorage = new FirebaseStorage(likeStorage);
            return await firebaseStorage
                .Child("pdf")
                .Child(fileName)
                .GetDownloadUrlAsync();
        }
    }
}
