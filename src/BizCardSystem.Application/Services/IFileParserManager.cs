using BizCardSystem.Domain.FileHelper;
using Microsoft.AspNetCore.Http;

namespace BizCardSystem.Application.Services;

public interface IFileParserManager
{
    List<FileParser> ParseFile(IFormFile file);
    byte[] CreateXMLFile(List<FileParser> data);
    byte[] CreateCSVFile(List<FileParser> data);
}