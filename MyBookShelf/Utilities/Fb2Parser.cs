using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace MyBookShelf.Utilities
{
    public static class Fb2Parser
    {
        /// <summary>
        /// Asynchronously parses an FB2 file and extracts the book title, author, description, and cover image.
        /// </summary>
        public static async Task<(string bookTitle, string bookAuthor, string bookDescription, string imagePath)> ParseFb2FileAsync(string fb2Path)
        {
            // Define the directory where extracted images will be stored
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }
            try
            {
                // Load the FB2 XML document
                XDocument doc = XDocument.Load(fb2Path);

                // Define XML namespaces used in FB2 files
                XNamespace ns = "http://www.gribuser.ru/xml/fictionbook/2.0";
                XNamespace link = "http://www.w3.org/1999/xlink";

                // Extract the book title
                string bookTitle = doc.Descendants(ns + "book-title").FirstOrDefault()?.Value ?? "Unknown Title";

                // Extract the author's first and last name
                var authorElement = doc.Descendants(ns + "author").FirstOrDefault();
                string bookAuthor = authorElement != null ?
                    ($"{authorElement.Element(ns + "first-name")?.Value} {authorElement.Element(ns + "last-name")?.Value}").Trim() : "Unknown Author";

                // Extract the book description (annotation)
                string bookDescription = doc.Descendants(ns + "annotation").FirstOrDefault()?.Value ?? "No description available";

                // Find the cover image element
                var coverElement = doc.Descendants(ns + "coverpage").FirstOrDefault();
                if (coverElement == null) return (bookTitle, bookAuthor, bookDescription, "");

                // Get the cover image ID from the "coverpage" element
                var coverId = coverElement.Elements(ns + "image")
                                          .Attributes(link + "href")
                                          .FirstOrDefault()?.Value.TrimStart('#');
                if (string.IsNullOrEmpty(coverId)) return (bookTitle, bookAuthor, bookDescription, "");

                // Find the binary data element that contains the image
                var binaryElement = doc.Descendants(ns + "binary")
                                       .FirstOrDefault(x => x.Attribute("id")?.Value == coverId);
                if (binaryElement == null) return (bookTitle, bookAuthor, bookDescription, "");

                // Extract and decode the base64-encoded image data
                string binaryData = binaryElement.Value.Replace("\n", "").Replace("\r", "");
                byte[] imageBytes = Convert.FromBase64String(binaryData);

                // Generate a unique filename for the extracted image
                string newFileName = Guid.NewGuid().ToString() + ".jpg";
                string destinationPath = Path.Combine(imagesFolder, newFileName);

                // Save the image asynchronously to the designated folder
                await File.WriteAllBytesAsync(destinationPath, imageBytes);

                // Return the extracted book details along with the saved image path
                return (bookTitle, bookAuthor, bookDescription, destinationPath);
            }
            catch (Exception)
            {
                // Return error placeholders in case of any exceptions
                return ("Error", "Error", "Error", "");
            }
        }
    }
}
