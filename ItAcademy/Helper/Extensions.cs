using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ItAcademy.Helper
{
    public static class Extensions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }
        public static bool IsMb(this IFormFile file)
        {
            return file.Length / 1024 > 1024 * 1;
        }
        public static async Task< string> SaveFileAsync(this IFormFile file, string folder)
        {
            string filename = Guid.NewGuid().ToString()+file.FileName;
            string path = Path.Combine(folder, filename);
            using (FileStream fileStream = new FileStream(path,FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }
    }
}
