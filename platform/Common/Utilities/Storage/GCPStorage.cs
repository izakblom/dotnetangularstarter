using Common.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.Utilities.Storage
{
    public class GCPStorage : CustomLoggerGCP, IStorage, IDisposable
    {
        //public GoogleCredential _credentials;


        public string ProjectId { get; set; }

        public GCPStorage(string projectId, IConfiguration configuration) : base(configuration)
        {
            ProjectId = projectId;
            // Get the Application Default Credentials.
            //_credentials = GoogleCredential.GetApplicationDefault();
            _route = this.GetType().FullName.ToString();
            _labels.Add("route", _route);
            _labels.Add("context", "commission");
        }

        public void Dispose()
        {
        }

        public async Task<string> UploadFileAsync(string bucket, string filename, byte[] file, string contentType)
        {
            using (var _client = Google.Cloud.Storage.V1.StorageClient.Create())
            {
                try
                {
                    var b = _client.CreateBucket(ProjectId, bucket);
                }
                catch (Exception)
                {
                    //Will throw exception if bucket already exists
                }

                try
                {
                    var imageObject = await _client.UploadObjectAsync(
                        bucket: bucket,
                        objectName: filename,
                        contentType: contentType,
                        source: new MemoryStream(file));

                    return imageObject.MediaLink;
                }
                catch (Exception e)
                {
                    LogException(e);
                    Console.WriteLine(e.StackTrace);
                }

                return null;
            }
        }

        public async Task<bool> DeleteObjectAsync(string bucket, string filename)
        {
            using (var _client = Google.Cloud.Storage.V1.StorageClient.Create())
            {
                try
                {
                    var b = _client.CreateBucket(ProjectId, bucket);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    await _client.DeleteObjectAsync(bucket, filename);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<byte[]> DownloadFileAsync(string bucket, string filename)
        {
            using (var _client = Google.Cloud.Storage.V1.StorageClient.Create())
            {
                try
                {
                    var b = _client.CreateBucket(ProjectId, bucket);
                }
                catch (Exception ex)
                {
                }

                using (var ms = new MemoryStream())
                {
                    await _client.DownloadObjectAsync(bucket, filename, ms);
                    return ms.ToArray();
                }
            }
        }
    }
}