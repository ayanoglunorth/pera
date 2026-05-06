using Pera.DTO.DTOs;
using System.Collections.Generic;

namespace Pera.Business.Abstract
{
    public interface IUploadedResultService
    {
        List<UploadedResultDto> GetAll(string teacherId);
        UploadedResultDto GetById(int id, string teacherId);
        void Upload(UploadResultDto model, string teacherId);
        void Delete(int id, string teacherId);
        string GetFilePath(int id, string teacherId);
    }

    public class UploadResultDto
    {
        public Microsoft.AspNetCore.Http.IFormFile File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}