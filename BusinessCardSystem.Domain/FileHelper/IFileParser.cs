using Microsoft.AspNetCore.Http;

namespace BizCardSystem.Domain.FileHelper;

public interface IFileParser
{
    List<FileParser> Parse(IFormFile file);
    bool CanParse(string extension);
}
