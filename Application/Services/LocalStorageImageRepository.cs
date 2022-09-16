using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public class LocalStorageImageRepository : IImageRepository
    {
        public async Task<string> Upload(IFormFile file, string fileName)
        {
           try
           {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Images", fileName);
                using Stream fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);
                return GetServerRelativePath(fileName);
           }
            catch (Exception e)
           {
                return null;
           }
        }

        private string GetServerRelativePath(string fileName)
        {
            return Path.Combine(@"Resources\Images", fileName);
        }
    }
}
