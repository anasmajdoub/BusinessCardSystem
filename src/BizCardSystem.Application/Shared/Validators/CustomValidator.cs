using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace BizCardSystem.Application.Shared.Validators
{
    public static class CustomValidator
    {
        public static bool BeValidBase64(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
                return false;


            var base64DataMatch = Regex.Match(base64Image, @"^data:(?:image|audio|video|application)\/[a-zA-Z]+;base64,");
            if (base64DataMatch.Success)
            {

                base64Image = base64Image.Substring(base64DataMatch.Length);
            }


            base64Image = base64Image.Trim();


            var base64Regex = new Regex(@"^[a-zA-Z0-9+/]*={0,2}$", RegexOptions.None);


            if (!base64Regex.IsMatch(base64Image))
                return false;

            try
            {
                Convert.FromBase64String(base64Image);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool BeLessThan1MB(string base64Image)
        {
            try
            {

                var base64DataMatch = Regex.Match(base64Image, @"^data:(?:image|audio|video|application)\/[a-zA-Z]+;base64,");
                if (base64DataMatch.Success)
                {
                    base64Image = base64Image.Substring(base64DataMatch.Length);
                }

                var decodedBytes = Convert.FromBase64String(base64Image);


                return decodedBytes.Length <= 1024 * 1024;
            }
            catch
            {

                return false;
            }
        }

        public static bool BeValidFileType(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var validExtensions = new[] { ".xml", ".csv", ".qr" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!Array.Exists(validExtensions, ext => ext == extension))
                return false;

            return true;
        }
    }
}
