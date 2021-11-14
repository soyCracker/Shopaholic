using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Imgur.API.Models;
using Microsoft.AspNetCore.Http;
using Shopaholic.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Services
{
    public class ImgurService : IStorageService
    {
        private string clientId;
        private string clientSecret;

        public ImgurService(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public async Task<bool> DeleteFile(string url)
        {
            ApiClient apiClient = new ApiClient(clientId, clientSecret);
            HttpClient httpClient = new HttpClient();
            ImageEndpoint imageEndpoint = new ImageEndpoint(apiClient, httpClient);
            var result = await imageEndpoint.DeleteImageAsync(url);
            return result;
        }

        public async Task<string> UploadFile(string folder, IFormFile file)
        {
            ApiClient apiClient = new ApiClient(clientId, clientSecret);
            HttpClient httpClient = new HttpClient();
            ImageEndpoint imageEndpoint = new ImageEndpoint(apiClient, httpClient);
            var imageUpload = await imageEndpoint.UploadImageAsync(file.OpenReadStream());
            return imageUpload.Link;
        }
    }
}
