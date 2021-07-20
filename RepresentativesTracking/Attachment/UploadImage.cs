using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
namespace RepresentativesTracking.Attachment {
    public class UploadImage {
        private readonly IHostingEnvironment _environment;

        public UploadImage(IHostingEnvironment environment) { _environment = environment; }

        public async Task<string> Upload(string bas64) {
            var strm = bas64;
            var filName = Guid.NewGuid();
            var filepath = _environment.ContentRootPath + @"/wwwroot/Images/" + filName + ".jpeg";
            var bytess = Convert.FromBase64String(strm);

            using (var fileStream = new FileStream(filepath, FileMode.Create)) {
                await fileStream.WriteAsync(bytess, 0, bytess.Length);
                await fileStream.FlushAsync();
            }

            return filName.ToString();
        }
        public async Task<byte[]> Download(string bas64)
        {
            var filepath = _environment.ContentRootPath + @"/wwwroot/Images/" + bas64 + ".jpeg";
            return File.ReadAllBytes(filepath);
        }
        public static bool IsBase64(string base64String) 
        {
            var ok = true;
            if (string.IsNullOrEmpty(base64String)) {
                ok = false;
            }

            if (base64String.Length % 4 != 0) {
                ok = false;
            }

            if (base64String.Contains(" ") && !base64String.Contains("\t")) {
                ok = false;
            }

            if (base64String.Contains("\r")) {
                ok = false;
            }

            if (base64String.Contains("\n")) {
                ok = false;
            }

            return ok;
        }
    }
}