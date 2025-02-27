using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Domain.Models;
using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignFileAsync([FromBody] AssignFileDto dto, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasModerationPermission = (permissions & Permission.ModerateUsers) == Permission.ModerateUsers;

            await _fileService.AssignFileAsync(dto, hasModerationPermission, cancellationToken);
            return Ok();
        }

        [HttpPost("assign-multiple")]
        public async Task<IActionResult> AssignFileAsync([FromBody] List<AssignFileDto> dto, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasModerationPermission = (permissions & Permission.ModerateUsers) == Permission.ModerateUsers;

            await _fileService.AssignFileAsync(dto, hasModerationPermission, cancellationToken);
            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteFileAsync([FromBody] Guid fileId, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasModerationPermission = (permissions & Permission.ModerateUsers) == Permission.ModerateUsers;

            await _fileService.DeleteFileAsync(fileId, hasModerationPermission, cancellationToken);
            return Ok();
        }

        [HttpPost("delete-multiple")]
        public async Task<IActionResult> DeleteFileAsync([FromBody] List<Guid> fileIds, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasModerationPermission = (permissions & Permission.ModerateUsers) == Permission.ModerateUsers;

            await _fileService.DeleteFileAsync(fileIds, hasModerationPermission, cancellationToken);
            return Ok();
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFileAsync([FromQuery] Guid fileId, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasPrivateContentAccess = (permissions & Permission.AccessPrivateContent) == Permission.AccessPrivateContent;

            var file = await _fileService.GetFileAsync(fileId, hasPrivateContentAccess, cancellationToken);
            return File(file.Stream, file.ContentType, file.FileName);
        }
    }
}
