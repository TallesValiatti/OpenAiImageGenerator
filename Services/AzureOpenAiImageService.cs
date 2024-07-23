using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using OpenAI.Images;
using OpenAiImageGenerator.Services.Models;

namespace OpenAiImageGenerator.Services;

public class AzureOpenAiImageService(IConfiguration configuration) : IImageService
{
    private ImageClient GetImageClient()
    {
        string endpoint = configuration["AzureOpenAi:Endpoint"]!;
        string key = configuration["AzureOpenAi:Key"]!;

        AzureOpenAIClient azureClient = new(new Uri(endpoint), new AzureKeyCredential(key));
        
        return azureClient.GetImageClient(configuration["AzureOpenAi:ImageModel"]!);
    }
    
    private ChatClient GetChatClient()
    {
        string endpoint = configuration["AzureOpenAi:Endpoint"]!;
        string key = configuration["AzureOpenAi:Key"]!;

        AzureOpenAIClient azureClient = new(new Uri(endpoint), new AzureKeyCredential(key));
        
        return azureClient.GetChatClient(configuration["AzureOpenAi:ChatModel"]!);
    }

    public async Task<ImageResult> CreateImageAsync(CreateImageRequest request)
    {
        var client = GetImageClient();
        
        var image = await client.GenerateImageAsync(
            request.GeneralIdea,
            new ImageGenerationOptions
            { 
                Quality = GeneratedImageQuality.Standard, 
                Size = GeneratedImageSize.W1024xH1024,
                Style = GeneratedImageStyle.Vivid,
                ResponseFormat = GeneratedImageFormat.Uri,
            }
        );

        return new ImageResult(image.Value!.ImageUri.ToString());
    }

    public async Task<DescribeImageResult> DescribeImageAsync(DescribeImageRequest request)
    {
        var client = GetChatClient();

        ChatMessageContentPart[] content =
        [
            ChatMessageContentPart.CreateImageMessageContentPart(new Uri(request.Url))
        ];
        
        ChatCompletion completion = await client.CompleteChatAsync(
        [
            new SystemChatMessage("You are a helpful assistant that create image descriptions"),
            new UserChatMessage(content)
        ]);

        var result = completion.Content[0].Text;
        
        return new DescribeImageResult(result);
    }
}