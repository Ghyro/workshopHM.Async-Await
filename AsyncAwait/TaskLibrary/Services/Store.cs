using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using TaskLibrary.Interfaces;

namespace TaskLibrary.Services
{
    public class Store : IStore
    {
        private readonly DirectoryInfo _directoryInfo;

        public Store(string pathToDirectory)
        {
            if (string.IsNullOrEmpty(pathToDirectory))
            {
                throw new ArgumentNullException(nameof(pathToDirectory));
            }

            this._directoryInfo = new DirectoryInfo(pathToDirectory);
        }

        #region Methods
        public string GetPath(Uri uri)
        {
            var result = Path.Combine(_directoryInfo.FullName, uri.Host) + uri.LocalPath.Replace("/", @"\");

            return result;
        }

        public string GetValidFileName(string fileName)
        {
            string pattern = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            Regex regex = new Regex(string.Format("[{0}]", Regex.Escape(pattern)));

            var result = regex.Replace(fileName, "");

            return result;
        }

        public async Task SaveAsync(Uri uri, Stream file)
        {
            string fileFullPath = GetPath(uri);

            var directoryPath = Path.GetDirectoryName(fileFullPath);

            await SaveToFile(file, fileFullPath);

            file.Close();
        }

        public async Task SaveHtmlAsync(Uri uri, HtmlDocument document)
        {
            var stream = new MemoryStream();

            document.Save(stream);

            stream.Seek(0, SeekOrigin.Begin);

            string directoryPath = GetPath(uri);

            Directory.CreateDirectory(directoryPath);

            var name = GetValidFileName(document.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText + ".html");

            string fileFullPath = Path.Combine(directoryPath, name);

            await SaveToFile(stream, fileFullPath);

            stream.Close();
        }

        public async Task SaveToFile(Stream stream, string filePath)
        {
            var currentStream = File.Create(filePath);

            await stream.CopyToAsync(currentStream);

            currentStream.Close();
        }
        #endregion
    }
}
