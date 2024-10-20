using BizCardSystem.Domain.FileHelper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace BizCardSystem.Infrastructure.Parsers;

public class CsvFileParser : IFileParser, IFilExport<FileParser>
{

    public List<FileParser> Parse(IFormFile file)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
            HeaderValidated = null,
        };

        using (var stream = file.OpenReadStream())
        using (var reader = new StreamReader(stream))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<FileParserMap>();

            try
            {
                var records = new List<FileParser>();
                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<FileParser>();
                        if (record != null)
                        {
                            records.Add(record);
                        }
                    }
                    catch (CsvHelperException ex)
                    {
                        Console.WriteLine($"Error parsing row: {ex.Message}");
                        // Continue to next record
                    }
                }
                return records;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing CSV: {ex.Message}");
                return new List<FileParser>();
            }
        }
    }
    //public List<FileParser> Parse(IFormFile file)
    //{
    //    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    //    {
    //        HasHeaderRecord = true,
    //        MissingFieldFound = null, // Ignore missing fields
    //        BadDataFound = null       // Ignore bad data
    //    };

    //    using (var stream = file.OpenReadStream())
    //    using (var reader = new StreamReader(stream))
    //    using (var csv = new CsvReader(reader, config))
    //    {
    //        csv.Context.RegisterClassMap<FileParserMap>();

    //        try
    //        {
    //            var records = csv.GetRecords<FileParser>().ToList();
    //            return records;
    //        }
    //        catch (CsvHelperException ex)
    //        {

    //            Console.WriteLine($"Error parsing CSV: {ex.Message}");
    //            return new List<FileParser>();
    //        }
    //    }
    //}

    //public List<FileParser> Parse(IFormFile file)
    //{
    //    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    //    {
    //        HasHeaderRecord = true,
    //        MissingFieldFound = null,
    //        BadDataFound = null,
    //        IgnoreBlankLines = false, // Changed to false to handle empty lines with delimiters
    //        TrimOptions = TrimOptions.Trim,
    //    };

    //    using var reader = new StreamReader(file.OpenReadStream());
    //    using var csv = new CsvReader(reader, config);

    //    csv.Context.RegisterClassMap<FileParserMap>();

    //    var results = new List<FileParser>();
    //    var errors = new List<string>();

    //    int rowIndex = 0;

    //    while (csv.Read())
    //    {
    //        csv.ReadHeader();
    //        rowIndex++;
    //        try
    //        {
    //            if (csv.Parser.Record.All(string.IsNullOrWhiteSpace))
    //            {
    //                Console.WriteLine($"Skipping empty row at index {rowIndex}");
    //                continue; // Skip empty rows, including those with only delimiters
    //            }
    //            var record = new FileParser
    //            {
    //                Id = csv.TryGetField<int>("Id", out var id) ? id : 0,
    //                Name = csv.GetField("Name") ?? string.Empty,
    //                Gender = csv.GetField("Gender") ?? string.Empty,
    //                DateofBirth = csv.GetField("DateofBirth") ?? string.Empty,
    //                Email = csv.GetField("Email") ?? string.Empty,
    //                Phone = csv.GetField("Phone") ?? string.Empty,
    //                Photo = csv.GetField("Photo") ?? string.Empty,
    //                Address = csv.GetField("Address") ?? string.Empty
    //            };

    //            results.Add(record);
    //        }
    //        catch (Exception ex)
    //        {
    //            errors.Add($"Error on row {rowIndex}: {ex.Message}");
    //            Console.WriteLine($"Problematic row content: {string.Join(", ", csv.Parser.Record)}");
    //        }
    //    }

    //    // Log or handle errors as needed
    //    foreach (var error in errors)
    //    {
    //        Console.WriteLine(error);  // Or use a proper logging framework
    //    }

    //    return results;
    //}
    public bool CanParse(string extension) => extension.Equals(".csv", StringComparison.OrdinalIgnoreCase);

    public byte[] Export(List<FileParser> data)
    {
        using (var memoryStream = new MemoryStream())
        using (var writer = new StreamWriter(memoryStream))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteHeader<FileParser>();
            csv.NextRecord();
            csv.WriteRecords(data);

            writer.Flush();
            //memoryStream.Position = 0;
            return memoryStream.ToArray();
        }
    }

    public class FileParserMap : ClassMap<FileParser>
    {
        public FileParserMap()
        {

            Map(m => m.Id).Name("Id");
            Map(m => m.Name).Name("Name");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.DateofBirth).Name("DateofBirth");
            Map(m => m.Email).Name("Email");
            Map(m => m.Phone).Name("Phone");
            Map(m => m.Photo).Name("Photo");
            Map(m => m.Address).Name("Address");
        }
    }

}

