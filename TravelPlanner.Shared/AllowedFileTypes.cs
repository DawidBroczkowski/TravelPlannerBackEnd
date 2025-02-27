using TravelPlanner.Domain.Models;

namespace TravelPlanner.Shared
{
    public static class AllowedFileTypes
    {
        public static readonly string[] ImageMimeTypes = { "image/jpeg", "image/png", "image/gif" };
        public static readonly string[] VideoMimeTypes = { "video/mp4", "video/quicktime", "video/x-msvideo", "video/x-ms-wmv" };
        public static readonly string[] DocumentMimeTypes = { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };

        public static bool IsValidFileType(string contentType)
        {
            return ImageMimeTypes.Contains(contentType) || VideoMimeTypes.Contains(contentType) || DocumentMimeTypes.Contains(contentType);
        }

        public static MediaType GetMediaType(string contentType)
        {
            if (ImageMimeTypes.Contains(contentType))
            {
                return MediaType.Images;
            }
            if (VideoMimeTypes.Contains(contentType))
            {
                return MediaType.Videos;
            }
            if (DocumentMimeTypes.Contains(contentType))
            {
                return MediaType.Videos;
            }
            return MediaType.Others;
        }

        public static string[] GetFileTypes(MediaType mediaType)
        {
            return mediaType switch
            {
                MediaType.Images => ImageMimeTypes,
                MediaType.Videos => VideoMimeTypes,
                MediaType.Documents => DocumentMimeTypes,
                _ => Array.Empty<string>()
            };
        }
    }

}
