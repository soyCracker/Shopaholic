using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFile(string folder, IFormFile file);

        Task<bool> DeleteFile(string url);
    }
}
