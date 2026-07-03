
    using Microsoft.AspNetCore.Http;

    namespace SchoolManagementSystem.Services
    {
        public class ImageService
        {
            private readonly IWebHostEnvironment _environment;


            public ImageService(IWebHostEnvironment environment)
            {
                _environment = environment;
            }


            public async Task<string?> SaveUserImageAsync(IFormFile? image)
            {
                if (image == null || image.Length == 0)
                    return null;


                // generate unique name
                var fileName = Guid.NewGuid().ToString()
                               + Path.GetExtension(image.FileName);


                // wwwroot/images/users
                var folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "users"
                );


                // create folder if not exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                var filePath = Path.Combine(folderPath, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }


                return fileName;
            }
        }
    }
