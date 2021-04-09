
using System;
using System.IO;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.Exceptions;

namespace Infrastructure.Photos
{
    public class PhotoAccessor : IPhotoAccessor
    {
        private readonly MinioClient _minioClient;
        private string BucketName { get; }
        private string AccessPoint {get;}
        public PhotoAccessor(IOptions<MinioSettings> config)
        {
            var acc = new MinioSettings
            {
                Endpoint = config.Value.Endpoint,
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret
            };
            
            _minioClient = new MinioClient(acc.Endpoint, acc.ApiKey, acc.ApiSecret);
            BucketName = config.Value.BucketName;
            AccessPoint = config.Value.AccessPoint;

            try
            {
                bool found = _minioClient.BucketExistsAsync(BucketName).Result;
                if (!found){
                    _minioClient.MakeBucketAsync(BucketName);
                    string policyJson = $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Action"":[""s3:GetObject""], ""Principal"":{{""AWS"":[""*""]}}, ""Effect"":""Allow"",""Resource"":[""arn:aws:s3:::{BucketName}/*""],""Sid"":""""}}]}}";
                    _minioClient.SetPolicyAsync(BucketName, policyJson);
                    Console.WriteLine($"Policy {policyJson} set for the bucket {BucketName} successfully");
                    Console.WriteLine();
                }
            }
            catch (MinioException e)
            {
                Console.WriteLine("Error occurred: " + e);
            }
        }

        public PhotoUploadResult AddPhoto(IFormFile file)
        {
            string photoId = Guid.NewGuid().ToString();
            string photoUrl = "http://" + AccessPoint + $"/{BucketName}/" + photoId;
            bool result = true;
            try
            {
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {   
                        result = _minioClient.PutObjectAsync(BucketName, photoId, stream, stream.Length, file.ContentType).IsCompletedSuccessfully;
                    }
                }
            }
            catch (MinioException e)
            {
                throw e;
            }

            return new PhotoUploadResult
            {
                PublicId = photoId,
                Url = photoUrl
            };
        }
        
        public bool DeletePhoto(string FileName)
        {
            return _minioClient.RemoveObjectAsync(BucketName, FileName).IsCompletedSuccessfully;

        }
    }
}