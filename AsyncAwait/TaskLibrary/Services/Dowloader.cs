using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskLibrary.Interfaces;

namespace TaskLibrary.Services
{
    public class Dowloader : IDowloader
    {
        #region entities
        private readonly IStore _saver;
        private readonly IExtensionConstrains _file;
        private readonly IConstrains _constrains;
        private readonly Uri _secondUri;
        private readonly CancellationToken _token;
        private readonly int _depth;
        private IList<Uri> _urls = new List<Uri>();
        #endregion

        #region Constructor
        public Dowloader(IStore saver, IExtensionConstrains file, IConstrains constrains, string url, int depth, CancellationToken token)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(nameof(url));
            }

            if (depth < 0)
            {
                throw new ArgumentException(nameof(depth));
            }            

            _constrains = constrains ?? throw new ArgumentNullException(nameof(constrains));

            _file = file ?? throw new ArgumentNullException(nameof(file));

            _saver = saver ?? throw new ArgumentNullException(nameof(saver));

            _token = token;

            _depth = depth;

            _secondUri = new Uri(url);
        }
        #endregion

        #region Methods
        public async Task LoadUrl()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _secondUri;

                await ScanUrlAsync(client, client.BaseAddress, 0);
            }
        }

        private async Task ScanUrlAsync(HttpClient client, Uri uri, int depth)
        {
            if (depth > _depth || _urls.Contains(uri) || !(uri.Scheme == "http" || uri.Scheme == "https"))
            {
                return;
            }

            _urls.Add(uri);

            var responseMessage = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri), _token);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return;
            }

            if (responseMessage.Content.Headers.ContentType?.MediaType == "text/html")
            {
                await ProcessHtmlDocumentAsync(client, uri, depth);
            }
            else
            {
                await ProcessFileAsync(client, uri);
            }
        }

        private async Task ProcessHtmlDocumentAsync(HttpClient client, Uri uri, int depth)
        {
            if (!_constrains.IsMatch(uri, _secondUri))
            {
                return;
            }

            HttpResponseMessage response = await client.GetAsync(uri, _token);

            var document = new HtmlDocument();

            document.Load(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);

            await _saver.SaveHtmlAsync(uri, document);

            var attributesWithLinks = document.DocumentNode.Descendants()
                .SelectMany(d => d.Attributes.Where(a => (a.Name == "src" || a.Name == "href")));

            foreach (var attributesWithLink in attributesWithLinks)
            {
                await ScanUrlAsync(client, new Uri(client.BaseAddress, attributesWithLink.Value), depth + 1);
            }
        }

        private async Task ProcessFileAsync(HttpClient client, Uri uri)
        {
            if (!_constrains.IsMatch(uri, _secondUri) || !_file.IsMatch(uri))
            {
                return;
            }

            HttpResponseMessage response = await client.GetAsync(uri, _token);

            await _saver.SaveAsync(uri, response.Content.ReadAsStreamAsync().Result);
        }
    }
    #endregion
}
