using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

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

                if (file.Length > 512 * 1024)
                {
                    using (var image = await Image.LoadAsync(file.OpenReadStream()))
                    {
                        var resizeOptions = new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(800, 800)
                        };
                        image.Mutate(x => x.Resize(resizeOptions));
                        var jpegOptions = new JpegEncoder { Quality = 75 };
                        await image.SaveAsync(filePath, jpegOptions);
                    }
                }
                else
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                res = "/uploads/" + uniqueFileName;
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }
            return res;
        }
        public async Task DeletePrevImg(string filePath)
        {
            try
            {
                
                File.Delete(filePath);
                Console.WriteLine($"Deleted file: {filePath}");
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {filePath}, Exception: {ex.Message}");
            }
        }

    }
}
