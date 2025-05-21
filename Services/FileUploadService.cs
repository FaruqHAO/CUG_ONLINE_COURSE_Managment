
using Microsoft.Extensions.Configuration;

namespace CUG_ONLINE_COURSES.Services
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileUploadService> _logger;
        private readonly IConfiguration _configuration;


        public FileUploadService(IWebHostEnvironment env, ILogger<FileUploadService> logger, IConfiguration configuration)
        {
            _env = env;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> SaveFileToServerAsync(Stream fileStream, string fileName, string fileCategory)
        {
            try
            {
                // Use /tmp for Render free plan (writable path)
                string tempStorageRoot = "/tmp";

                // Combine /tmp with category (if any)
                string uploadsFolder = Path.Combine(tempStorageRoot, fileCategory ?? "uploads");

                // Log the path for debugging
                _logger.LogInformation($"Uploads folder: {uploadsFolder}");

                // Ensure the uploads folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Combine folder path with file name
                string filePath = Path.Combine(uploadsFolder, fileName);

                // Save the file
                using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStreamOutput);
                }

                _logger.LogInformation($"File saved successfully: {filePath}");

                // Return the physical path or public URL if you later move this to a cloud storage
                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError($"File upload failed: {ex.Message}");
                throw;
            }
        }

        //public async Task<string> SaveFileToServerAsync(Stream fileStream, string fileName, string FileCategorie)
        //{
        //    try
        //    {
        //        // Get the file storage path from the configuration
        //        string fileStoragePath = _configuration["FileStorage:FileStoragePath"];

        //        // Log the file storage path for debugging
        //        _logger.LogInformation($"File storage path: {fileStoragePath}");

        //        // Ensure that the file storage path is not null or empty
        //        if (string.IsNullOrEmpty(fileStoragePath))
        //        {
        //            throw new InvalidOperationException("File storage path is not configured properly.");
        //        }

        //        // Combine the file storage path with the WebRootPath to create the full folder path
        //        string uploadsFolder = Path.Combine(_env.WebRootPath, fileStoragePath);

        //        // Log the full uploads folder path
        //        _logger.LogInformation($"Uploads folder: {uploadsFolder}");

        //        // Ensure the uploads folder exists
        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        // Combine the folder path with the file name
        //        string filePath = Path.Combine(uploadsFolder, fileName);

        //        // Log the full file path
        //        _logger.LogInformation($"File path: {filePath}");

        //        // Save the file to the server
        //        using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
        //        {
        //            await fileStream.CopyToAsync(fileStreamOutput);
        //        }

        //        _logger.LogInformation($"File saved successfully: {filePath}");

        //        // Return the relative path (URL) for the file to use in the Blazor component
        //        return $"/{fileStoragePath}{fileName}"; // Remove 'wwwroot' if needed
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"File upload failed: {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<string> SaveFileToServerAsync(Stream fileStream, string fileName,string FileCategorie)
        //{
        //    try
        //    {
        //       // string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
        //        // Get the file storage path from the configuration
        //        string fileStoragePath = _configuration["FileStorage:FileStoragePath"];


        //        // Ensure the uploads folder exists
        //        string uploadsFolder = Path.Combine(_env.WebRootPath, fileStoragePath);
        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        string filePath = Path.Combine(uploadsFolder, fileName);

        //        // Save the file to the server
        //        using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
        //        {
        //            await fileStream.CopyToAsync(fileStreamOutput);
        //        }

        //        _logger.LogInformation($"File saved successfully: {filePath}");

        //        // Return the relative path (URL) for the file to use in the Blazor component
        //        return $"/{fileStoragePath.Replace("wwwroot", "")}{fileName}"; // This removes "wwwroot" from the path

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"File upload failed: {ex.Message}");
        //        throw;
        //    }
        //}



    }
}
