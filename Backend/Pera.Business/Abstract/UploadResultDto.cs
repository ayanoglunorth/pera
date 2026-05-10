using Microsoft.AspNetCore.Http;

namespace Pera.Business.Abstract
{
    /// <summary>
    /// DTO for file upload requests. Lives in Business layer because it depends on IFormFile (ASP.NET Core).
    /// </summary>
    public class UploadResultDto
    {
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
