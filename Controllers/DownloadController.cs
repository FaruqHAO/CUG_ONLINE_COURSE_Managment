using Microsoft.AspNetCore.Mvc;

namespace CUG_ONLINE_COURSES.Controllers
{
    [Route("download")]
    public class DownloadController : Controller
    {
        [HttpGet("{*relativePath}")]
        public IActionResult Download(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return BadRequest("File path is missing.");

            // Prevent path traversal
            relativePath = relativePath.Replace("..", "");

            string filePath = Path.Combine("/tmp", relativePath);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var contentType = "application/octet-stream";
            var fileName = Path.GetFileName(filePath);

            return PhysicalFile(filePath, contentType, fileName);
        }
    }
}
