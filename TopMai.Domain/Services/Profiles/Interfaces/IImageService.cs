using Infraestructure.Entity.Entities.Profiles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static TopMai.Domain.Services.Profiles.ImageService;

namespace TopMai.Domain.Services.Profiles.Interfaces
{
    public interface IImageService
    {
        List<Image> GetAll();
        Image Get(Guid id);
        object Post(Image image);
        object Put(Image newImage);
        ImageResponse ReadImage(string path, IHostingEnvironment _env);
        Task<Image> UploadImageFile(string id, IFormFile file, IHostingEnvironment _env);

        object UploadPDF(string id,IFormFile file);
        ImageResponse ReadPDF(string path, IHostingEnvironment _env);
        object Delete(Guid id);
        string GetUrlImage(Image? Image);
    }
}
