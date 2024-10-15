using BizCardSystem.Domain.FileHelper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Drawing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace BizCardSystem.Infrastructure.Parsers;

public class QrCodeFileParser : IFileParser
{
    public List<FileParser> Parse(IFormFile file)
    {
        var businessCards = new List<FileParser>();
        using (var stream = file.OpenReadStream())
        {
            using (var bitmap = new Bitmap(stream))
            {
                var luminanceSource = new BitmapLuminanceSource(bitmap);
                var reader = new BarcodeReader
                {
                    AutoRotate = true,
                    Options = new DecodingOptions { TryHarder = true }
                };

                var result = reader.Decode(luminanceSource);

                if (result == null)
                {
                    throw new InvalidDataException("No QR code detected or it couldn't be decoded.");
                }

                try
                {
                    var card = JsonConvert.DeserializeObject<FileParser>(result.Text);
                    businessCards.Add(card);
                }
                catch (JsonException ex)
                {
                    throw new InvalidDataException("QR code content is not in the correct format for a BusinessCard.", ex);
                }
            }
        }
        return businessCards;
    }
    public bool CanParse(string extension) => extension.Equals(".qr", StringComparison.OrdinalIgnoreCase);
}

