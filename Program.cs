using OpenAiImageGenerator.Services;
using OpenAiImageGenerator.Services.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IImageService, AzureOpenAiImageService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/image", async (CreateImageRequest request, IImageService service)
                => await service.CreateImageAsync(request))
.WithName("CreateImage")
.WithOpenApi();

app.MapPost("/image/describe", async (DescribeImageRequest request, IImageService service)
        => await service.DescribeImageAsync(request))
    .WithName("DescribeImage")
    .WithOpenApi();

app.Run();