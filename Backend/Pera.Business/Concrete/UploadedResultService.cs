using Pera.Business.Abstract;
using Pera.DataAccess.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pera.Business.Concrete
{
    public class UploadedResultService : IUploadedResultService
    {
        private readonly IUploadedResultRepository _uploadedResultRepository;
        private readonly string _uploadFolder;

        public UploadedResultService(IUploadedResultRepository uploadedResultRepository)
        {
            _uploadedResultRepository = uploadedResultRepository;
            _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_uploadFolder))
                Directory.CreateDirectory(_uploadFolder);
        }

        public List<UploadedResultDto> GetAll(string teacherId)
        {
            var results = _uploadedResultRepository.GetByTeacher(teacherId);
            return results.Select(MapToDto).ToList();
        }

        public UploadedResultDto GetById(int id, string teacherId)
        {
            var result = _uploadedResultRepository.GetById(id);
            if (result == null || result.TeacherId != teacherId)
                return null;
            return MapToDto(result);
        }

        public void Upload(UploadResultDto model, string teacherId)
        {
            if (model.File == null || model.File.Length == 0)
                return;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.FileName);
            var filePath = Path.Combine(_uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }

            var uploadedResult = new UploadedResult
            {
                TeacherId = teacherId,
                FileName = model.FileName,
                StoredFileName = fileName,
                FileType = model.ContentType,
                FileSize = model.File.Length,
                UploadedAt = DateTime.Now
            };
            _uploadedResultRepository.Add(uploadedResult);
        }

        public void Delete(int id, string teacherId)
        {
            var result = _uploadedResultRepository.GetById(id);
            if (result == null || result.TeacherId != teacherId)
                return;

            var filePath = Path.Combine(_uploadFolder, result.StoredFileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            _uploadedResultRepository.Delete(id);
        }

        public string GetFilePath(int id, string teacherId)
        {
            var result = _uploadedResultRepository.GetById(id);
            if (result == null || result.TeacherId != teacherId)
                return null;
            return Path.Combine(_uploadFolder, result.StoredFileName);
        }

        private UploadedResultDto MapToDto(UploadedResult result)
        {
            return new UploadedResultDto
            {
                Id = result.Id,
                FileName = result.FileName,
                FileType = result.FileType,
                FileSize = result.FileSize,
                UploadedAt = result.UploadedAt
            };
        }
    }
}