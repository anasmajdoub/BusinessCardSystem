using BizCardSystem.Domain.FileHelper;
using Microsoft.AspNetCore.Http;
using System.Xml.Serialization;

namespace BizCardSystem.Infrastructure.Parsers;

public class XmlFileParser : IFileParser, IFilExport<FileParser>
{
    public List<FileParser> Parse(IFormFile file)
    {
        var cards = new List<FileParser>();
        using (var stream = file.OpenReadStream())
        {
            var serializer = new XmlSerializer(typeof(FileParserList));
            var result = (FileParserList)serializer.Deserialize(stream);

            return result.FileParsers;
        }
    }

    public bool CanParse(string extension) => extension.Equals(".xml", StringComparison.OrdinalIgnoreCase);

    public byte[] Export(List<FileParser> data)
    {
        var serializer = new XmlSerializer(typeof(List<FileParser>));
        using (var memoryStream = new MemoryStream())
        {
            serializer.Serialize(memoryStream, data);
            memoryStream.Position = 0;
            return memoryStream.ToArray();
        }
    }
}

