using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities.Storage
{
    public interface IStorage
    {
        // File Upload
        // File Download
        // File Delete

        Task<string> UploadFileAsync(string bucket, string filename, byte[] file, string contentType);
        Task<byte[]> DownloadFileAsync(string bucket, string filename);
        Task<bool> DeleteObjectAsync(string bucket, string filename);

    }
}
