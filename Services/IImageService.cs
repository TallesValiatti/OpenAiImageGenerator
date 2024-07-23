using OpenAiImageGenerator.Services.Models;

namespace OpenAiImageGenerator.Services;

public interface IImageService
{
    Task<ImageResult> CreateImageAsync(CreateImageRequest request);
    Task<DescribeImageResult> DescribeImageAsync(DescribeImageRequest request);
}