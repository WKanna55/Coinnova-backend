using Coinnova.Domain.Common.Models;

namespace Coinnova.Domain.Interfaces.Common;

public interface ICloudStorageService
{
    Task<string?> UploadImageAsync(FileUpload file);
    Task<string?> UploadRawFileAsync(FileUpload file);
}