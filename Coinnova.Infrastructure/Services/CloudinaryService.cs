using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Coinnova.Domain.Common.Models;
using Coinnova.Domain.Interfaces.Common;

namespace Coinnova.Infrastructure.Services;

public class CloudinaryService : ICloudStorageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService()
    {
        var account = new Account(
            Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
            Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
            Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET"));

        _cloudinary = new Cloudinary(account);
    }

    public async Task<string?> UploadImageAsync(FileUpload file)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.Content),
            Folder = file.Folder,
            PublicId = Path.GetFileNameWithoutExtension(file.FileName),
            Transformation = new Transformation().FetchFormat("webp").Quality("auto")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }

    public async Task<string?> UploadRawFileAsync(FileUpload file)
    {
        var uploadParams = new RawUploadParams
        {
            File = new FileDescription(file.FileName, file.Content),
            Folder = file.Folder,
            PublicId = Path.GetFileNameWithoutExtension(file.FileName)
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }
}