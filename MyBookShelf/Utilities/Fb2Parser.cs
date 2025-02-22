using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBookShelf.Utilities
{
    public static class Fb2Parser
    {
        public static async Task<(string bookTitle, string bookAuthor, string imagePath)> ParseFb2FileAsync(string fb2Path)
        {
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            try
            {
                XDocument doc = XDocument.Load(fb2Path);
                XNamespace ns = "http://www.gribuser.ru/xml/fictionbook/2.0";
                XNamespace link = "http://www.w3.org/1999/xlink";

                string bookTitle = doc.Descendants(ns + "book-title").FirstOrDefault()?.Value ?? "Unknown Title";

                var authorElement = doc.Descendants(ns + "author").FirstOrDefault();
                string bookAuthor = authorElement != null ?
                    ($"{authorElement.Element(ns + "first-name")?.Value} {authorElement.Element(ns + "last-name")?.Value}").Trim() : "Unknown Author";

                var coverElement = doc.Descendants(ns + "coverpage").FirstOrDefault();
                if (coverElement == null) return (bookTitle, bookAuthor, "");

                var coverId = coverElement.Elements(ns + "image")
                                          .Attributes(link + "href")
                                          .FirstOrDefault()?.Value.TrimStart('#');
                if (string.IsNullOrEmpty(coverId)) return (bookTitle, bookAuthor, "");

                var binaryElement = doc.Descendants(ns + "binary")
                                       .FirstOrDefault(x => x.Attribute("id")?.Value == coverId);
                if (binaryElement == null) return (bookTitle, bookAuthor, "");

                string binaryData = binaryElement.Value.Replace("\n", "").Replace("\r", "");
                byte[] imageBytes = Convert.FromBase64String(binaryData);

                string newFileName = Guid.NewGuid().ToString() + ".jpg";
                string destinationPath = Path.Combine(imagesFolder, newFileName);
                await File.WriteAllBytesAsync(destinationPath, imageBytes);

                return (bookTitle, bookAuthor, destinationPath);
            }
            catch (Exception)
            {
                return ("Error", "Error", "");
            }
        }
    }
}
