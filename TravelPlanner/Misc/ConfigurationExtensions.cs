using System.Security.Claims;
using System.Text;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared;
using TravelPlanner.Shared.DTOs.File;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;

namespace TravelPlanner.Misc
{
    public static class ConfigurationExtensions
    {
        static string _uploadPath = "E:\\repos\\TravelPlanner\\TravelPlanner\\Uploads"; // TODO: move to config
        public static void ConfigureTus(this WebApplication app)
        {
            app.UseTus(httpContext => new DefaultTusConfiguration
            {
                // Configure tusdotnet to use dynamic storage based on file type and date
                Store = new TusDiskStore("E:\\repos\\TravelPlanner\\TravelPlanner\\Uploads"),

                // Specify the URL path for file uploads
                UrlPath = "/files",

                // Set file expiration time
                Expiration = new AbsoluteExpiration(TimeSpan.FromDays(1)),
                MaxAllowedUploadSizeInBytes = 20000000,

                Events = new Events
                {
                    OnCreateCompleteAsync = ctx =>
                    {
                        var uploadUrl = ctx.HttpContext.Request.Path + "/" + ctx.FileId;
                        app.Logger.LogInformation($"Upload session created at {uploadUrl}");
                        return Task.CompletedTask;
                    },

                    OnFileCompleteAsync = async ctx =>
                    {
                        using var scope = app.Services.CreateScope();
                        {
                            var _fileDataRepository = scope.ServiceProvider.GetRequiredService<IFileDataRepository>();

                            // Retrieve the uploaded file information
                            var file = await ctx.GetFileAsync();

                            // Access the file's metadata (e.g., filename, content type)
                            var metadata = await file.GetMetadataAsync(default);

                            // Extract specific metadata (filename, content type)
                            var filename = metadata.ContainsKey("filename")
                                ? metadata["filename"].GetString(System.Text.Encoding.UTF8)
                                : "unknown";
                            var contentType = metadata.ContainsKey("contentType")
                                ? metadata["contentType"].GetString(System.Text.Encoding.UTF8)
                                : "unknown";

                            // Construct the original file path
                            var filePath = Path.Combine(@"E:\repos\TravelPlanner\TravelPlanner\Uploads", ctx.FileId);
                            if (!File.Exists(filePath))
                            {
                                throw new FileNotFoundException($"File not found at {filePath}");
                            }

                            // Save metadata to the database
                            var userIdClaim = ctx.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                            int.TryParse(userIdClaim?.Value, out int userId);

                            var fileMetadata = new FileDataDto
                            {
                                FileId = Guid.Parse(ctx.FileId),
                                FileName = filename,
                                ContentType = contentType,
                                Size = new FileInfo(filePath).Length,
                                UploadedAt = DateTime.Now,
                                UploadedById = userId,
                                Extension = Path.GetExtension(filename)
                            };

                            await _fileDataRepository.AddFileDataAsync(fileMetadata, default);

                            // Add the new file ID (GUID) to the response headers
                            ctx.HttpContext.Response.Headers.Append("Upload-FileId", fileMetadata.FileId.ToString());
                        }
                    }
                }
            });
        }

        private static string GetDynamicStoragePath(HttpContext context)
        {
            // Retrieve metadata from the "Upload-Metadata" header
            var uploadMetadata = context.Request.Headers["Upload-Metadata"].ToString();

            // Parse and decode the metadata
            var metadataDictionary = uploadMetadata
                .Split(',')
                .Select(part => part.Split(' '))
                .Where(parts => parts.Length == 2) // Ensure valid key-value pair
                .ToDictionary(
                    parts => parts[0],
                    parts => Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]))
                );

            // Extract contentType or default to "unknown"
            var contentType = metadataDictionary.ContainsKey("contentType")
                ? metadataDictionary["contentType"]
                : "unknown";

            // Use AllowedFileTypes to determine the media type (e.g., Images, Videos)
            var mediaType = AllowedFileTypes.GetMediaType(contentType).ToString();

            // Create a date-based folder structure (e.g., /Uploads/Images/2024/09/)
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString("D2");

            // Combine paths
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", mediaType, year, month);

            // Ensure the directory exists
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            Console.WriteLine($"TusDiskStore path: {fullPath}");

            return fullPath;
        }

    }
}
