using Microsoft.AspNetCore.Http;

namespace BizCardSystem.Application.BusinessCards.Dtos.File;

public class CreateFromFileRequest
{
    public IFormFile File { get; set; }
}
