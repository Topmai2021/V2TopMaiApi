using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Common.Utils.Resources;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using TopMai.Domain.Services.Profiles.Interfaces;
using User = Infraestructure.Entity.Entities.Users.User;

namespace TopMai.Domain.Services.Profiles
{
    public class ImageService : IImageService
    {
        #region Attributes

        private DataContext DBContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        #endregion

        #region Builder

        public ImageService(DataContext dBContext, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            DBContext = dBContext;
            _unitOfWork = unitOfWork;
            _config = configuration;
        }

        #endregion


        #region Methods

        public List<Infraestructure.Entity.Entities.Profiles.Image> GetAll() =>
            DBContext.Images.OrderByDescending(x => x.Id).ToList();

        public Infraestructure.Entity.Entities.Profiles.Image Get(Guid id) =>
            DBContext.Images.FirstOrDefault(p => p.Id == id);

        public object Post(Infraestructure.Entity.Entities.Profiles.Image image)
        {

            image.Id = Guid.NewGuid();
            image.Deleted = false;
            if (image.UrlImage == null || image.UrlImage.Length < 6)
                throw new BusinessException("La url de la imagen debe ser de al menos 5 caracteres ");

            DBContext.Images.Add(image);
            DBContext.SaveChanges();

            return DBContext.Images.Where(p => p.Id == image.Id).First();
        }

        public object Put(Infraestructure.Entity.Entities.Profiles.Image newImage)
        {
            var idImage = newImage.Id;
            if (idImage == null || idImage.ToString().Length < 6)
                throw new BusinessException("Ingrese un id de imagen válido ");

            Infraestructure.Entity.Entities.Profiles.Image? image =
                DBContext.Images.Where(c => c.Id == idImage && newImage.Id != null).FirstOrDefault();
            if (image == null)
                throw new BusinessException("El id no coincide con ninguna imagen ");

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newImage.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newImage) != null && propertyInfo.GetValue(newImage).ToString() != "")
                {
                    propertyInfo.SetValue(image, propertyInfo.GetValue(newImage));
                }
            }

            DBContext.Entry(image).State = EntityState.Modified;
            DBContext.SaveChanges();

            return image;
        }

        public ImageResponse ReadImage(string path, Microsoft.AspNetCore.Hosting.IHostingEnvironment _env)
        {
            var imageType = "image/" + path.Split('.')[1];
            var filePath = Path.Combine(_env.ContentRootPath, "../../../../Files/" + path);
            return new ImageResponse { filePath = filePath, imageType = imageType };

        }



        public object Delete(Guid id)
        {

            Infraestructure.Entity.Entities.Profiles.Image image = DBContext.Images.FirstOrDefault(p => p.Id == id);
            if (image == null) throw new BusinessException("El id ingresado no es válido ");

            image.Deleted = true;
            DBContext.Entry(image).State = EntityState.Modified;
            DBContext.SaveChanges();
            return image;
        }

        public object UploadPDF(string id, IFormFile file)
        {
            User user = DBContext.Users.FirstOrDefault(p => p.Id.ToString() == id);
            if (user == null || user.ProfileId == null)
            {
                if (id != "subcategories" && id != "admin")
                {
                    return new { error = "Las credenciales no son válidas" };

                }
            }
            if (file == null || file.Length == 0)
                throw new BusinessException("No se ha seleccionado ningún archivo ");

            if (file.FileName.Split('.').Last().ToLower() != "pdf")
                throw new BusinessException("El archivo seleccionado no es un pdf");

            var path = Path.Combine("../../../../Files/" + user.ProfileId.ToString() + "/");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = file.Name;
            var filePath = Path.Combine(path, fileName);
            string additionalName = "";
            while (System.IO.File.Exists(filePath))
            {
                additionalName += "(1)";
                filePath = "../../../../Files/" + user.ProfileId.ToString() + "/" + additionalName + file.FileName;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return new { url = "api2.topmai.app:1212/api/image/readPDF?path=" + user.ProfileId.ToString() + "/" + additionalName + fileName };
            //return new { url = "topmai.app/api/image/readPDF?path="+user.ProfileId.ToString()+"/"+additionalName+fileName };
        }

        public ImageResponse ReadPDF(string path, Microsoft.AspNetCore.Hosting.IHostingEnvironment _env)
        {
            var filePath = Path.Combine("../../../../Files/" + path);
            var imageType = "application/pdf";
            return new ImageResponse { filePath = filePath, imageType = imageType };

        }

        public async Task<Infraestructure.Entity.Entities.Profiles.Image> UploadImageFile(string id, IFormFile file, Microsoft.AspNetCore.Hosting.IHostingEnvironment _env)
        {
            User user = _unitOfWork.UserRepository.FirstOrDefault(x => x.Id.ToString() == id);
            if (user == null)
            {
                if (id != "subcategories" && id != "admin")
                    throw new BusinessException("Las credenciales no son válidas");
            }

            if (file == null)
                throw new BusinessException("No se ha seleccionado ningun archivo");
            if (file.Length == 0)
                throw new BusinessException("El archivo seleccionado esta vacio");
            if (file.Length > 3000000)
                throw new BusinessException("El archivo seleccionado es demasiado grande ( máximo 3 mb ) ");


            //Valido primero todas las extensiones de los archivos antes de mandar a guardar en nube.
            string extension = Path.GetExtension(file.FileName);
            if (!Helper.ValidImageExtencion(extension))
                throw new BusinessException(GeneralMessages.ImgExtension);


            string path = _config.GetSection("GeneralSettings").GetSection("PathImagen").Value;
            string uploads = Path.Combine(_env.WebRootPath, $"{path}{id.ToString()}/");
            if (!Directory.Exists(uploads))
            {
                Console.WriteLine("Creando el directorio: {0}", uploads);
                Directory.CreateDirectory(uploads);
            }

            string uniqueFileName = Helper.GetUniqueFileName(file.FileName);
            string filePath = $"{uploads}{uniqueFileName}";
            string filePath2 = $"{uploads}/compressed{uniqueFileName}";

            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            using (Bitmap bmp1 = new Bitmap(filePath))
            {


                if (extension == ".png" && id != "subcategories" && id != "admin")
                {
                    Bitmap bmp = new Bitmap(bmp1.Width, bmp1.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.White);
                        g.DrawImage(bmp1, new Rectangle(new Point(), bmp1.Size), new Rectangle(new Point(), bmp1.Size), GraphicsUnit.Pixel);
                    }
                    bmp.Save(filePath2, ImageFormat.Jpeg);
                }
                else
                {
                    ImageFormat format = GetFormatImg(extension);
                    ImageCodecInfo encoder = GetEncoder(format);

                    // Create an Encoder object based on the GUID  
                    // for the Quality parameter category.  
                    Encoder myEncoder = Encoder.Quality;

                    // Create an EncoderParameters object.  
                    // An EncoderParameters object has an array of EncoderParameter  
                    // objects. In this case, there is only one  
                    // EncoderParameter object in the array.  
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);


                    // Save the bitmap as a file with zero quality level compression.  
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 40L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    bmp1.Save(filePath2, encoder, myEncoderParameters);
                }
            }


            File.Delete(filePath);

            Infraestructure.Entity.Entities.Profiles.Image image = new Infraestructure.Entity.Entities.Profiles.Image()
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                UrlImage = $"{id.ToString()}/compressed{uniqueFileName}",
                CreationDate = DateTime.Now,
            };

            if (id != "subcategories" && id != "admin")
                image.ProfileId = Guid.Parse(id);



            _unitOfWork.ImageRepository.Insert(image);
            await _unitOfWork.Save();

            image.UrlImage = GetUrlImage(image);

            return image;


        }

        private ImageFormat GetFormatImg(string extension)
        {
            ImageFormat format;
            switch (extension)
            {
                case ".jpg":
                    format = ImageFormat.Jpeg;
                    break;

                case ".bmp":
                    format = ImageFormat.Bmp;
                    break;
                case ".png":
                    format = ImageFormat.Png;
                    break;
                case ".gif":
                    format = ImageFormat.Gif;
                    break;
                default:
                    format = ImageFormat.Jpeg;
                    break;
            }

            return format;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }


        public string GetUrlImage(Infraestructure.Entity.Entities.Profiles.Image? Image)
        {
            string img = string.Empty;
            if (Image != null && !string.IsNullOrEmpty(Image.UrlImage))
            {
                if (Image.CreationDate != null)
                {
                    string serverPath = _config.GetSection("GeneralSettings").GetSection("ServerPath").Value;
                    string pathImagen = _config.GetSection("GeneralSettings").GetSection("PathImagen").Value;
                    img = $"{serverPath}{pathImagen}{Image.UrlImage}";
                }
                else
                    img = Image.UrlImage;
            }
            else
            {
                string serverPath = _config.GetSection("GeneralSettings").GetSection("ServerPath").Value;
                string notAvailable = _config.GetSection("GeneralSettings").GetSection("PathImagNotAvailable").Value;
                img = $"{serverPath}{notAvailable}";
            }

            return img;
        }
        #endregion


    }
}

public class ImageResponse
{
    public string imageType { get; set; }
    public string filePath { get; set; }
}

