using Microsoft.AspNetCore.Hosting;

namespace EmployeeManagement.Service.FileService
{
    public class FileService:IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvinronment;

        public FileService(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvinronment= webHostEnvironment;
        }
        public async Task<string> SaveProfileImage(IFormFile file)
        {
            string res=string.Empty;
            try
            {
                var uploadsFolder = Path.Combine(_webHostEnvinronment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                res = "/uploads/" + uniqueFileName;
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }
            return res;
        }
    }
}
