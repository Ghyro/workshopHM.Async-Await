using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLibrary.Interfaces
{
    public interface IStore
    {
        Task SaveAsync(Uri uri, Stream file);

        Task SaveHtmlAsync(Uri uri, HtmlDocument document);

        Task SaveToFile(Stream stream, string filePath);

        string GetPath(Uri uri);

        string GetValidFileName(string fileName);
    }
}
