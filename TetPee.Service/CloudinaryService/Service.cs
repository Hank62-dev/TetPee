using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TetPee.Service.MediaService;

namespace TetPee.Service.CloudinaryService;

public class Service : IService
{
    
    private readonly Cloudinary _cloudinary;
    private readonly CloudinaryOptions _cloudinaryOptions = new();

    public Service(IConfiguration config)
    {
        config.GetSection(nameof(_cloudinaryOptions)).Bind(_cloudinaryOptions);
        _cloudinary = new Cloudinary(new Account(
            _cloudinaryOptions.CloudName,
            _cloudinaryOptions.ApiKey,
            _cloudinaryOptions.ApiSecret
            )
        );
    }
    
    public async Task<string> UpLoadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is null or empty", nameof(file));
        }
        if (!IsImageFile(file))
        {
            throw new ArgumentException("File is not a valid image", nameof(file));
        }
        
        await using var stream = file.OpenReadStream();
        var upLoadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream)
        };
        
        var uploadResult = await _cloudinary.UploadAsync(upLoadParams);
        return  uploadResult.SecureUri.ToString();
    }


    private bool IsImageFile(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        return allowedExtensions.Contains(fileExtension);
    }
}