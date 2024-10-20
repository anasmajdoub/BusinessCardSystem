using System.Xml.Serialization;

namespace BizCardSystem.Domain.FileHelper;

public class FileParser
{

    public int? Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string DateofBirth { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Photo { get; set; }
    public string Address { get; set; }
}

[XmlRoot("ArrayOfFileParser")]
public class FileParserList
{
    [XmlElement("FileParser")]
    public List<FileParser> FileParsers { get; set; }
}


