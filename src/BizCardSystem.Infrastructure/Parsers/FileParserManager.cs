using BizCardSystem.Application.Services;
using BizCardSystem.Domain.FileHelper;
using Microsoft.AspNetCore.Http;

namespace BizCardSystem.Infrastructure.Parsers;

public class FileParserManager : IFileParserManager
{
    private readonly IEnumerable<IFileParser> _parsers;
    private readonly IEnumerable<IFilExport<FileParser>> _exploers;

    public FileParserManager(IEnumerable<IFileParser> parsers, IEnumerable<IFilExport<FileParser>> exploers)
    {
        _parsers = parsers;
        _exploers = exploers;
    }

    public List<FileParser> ParseFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();
        var parser = _parsers.FirstOrDefault(p => p.CanParse(extension));

        if (parser == null)
            throw new NotSupportedException($"File format {extension} is not supported.");

        return parser.Parse(file);
    }

    public byte[] CreateXMLFile(List<FileParser> data)
    {
        var parser = _exploers.FirstOrDefault(p => p.CanParse(".xml"));

        if (parser == null)
            throw new NotSupportedException($"File format .xml is not supported.");

        return parser.Export(data);
    }

    public byte[] CreateCSVFile(List<FileParser> data)
    {
        var parser = _exploers.FirstOrDefault(p => p.CanParse(".csv"));

        if (parser == null)
            throw new NotSupportedException($"File format .xml is not supported.");

        return parser.Export(data);
    }
}