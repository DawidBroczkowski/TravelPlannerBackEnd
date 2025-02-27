using TravelPlanner.Domain.Models;
using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Shared.Extensions
{
    public static class FileDataExtensions
    {
        public static string GetPath(this FileData fileData)
        {
            //var mediaType = AllowedFileTypes.GetMediaType(fileData.ContentType).ToString();
            //var year = fileData.UploadedAt.Year.ToString();
            //var month = fileData.UploadedAt.Month.ToString("D2");
            return $"{fileData.FileId.ToString("N")}";
        }

        public static string GetPath(this FileDataDto fileData)
        {
            //var mediaType = AllowedFileTypes.GetMediaType(fileData.ContentType).ToString();
            //var year = fileData.UploadedAt.Year.ToString();
            //var month = fileData.UploadedAt.Month.ToString("D2");
            return $"{fileData.FileId.ToString("N")}";
        }
    }
}
