namespace LMS.API.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadMaterialAsync(IFormFile file);

        Task<string> UploadSubmissionAsync(IFormFile file);
    }
}
