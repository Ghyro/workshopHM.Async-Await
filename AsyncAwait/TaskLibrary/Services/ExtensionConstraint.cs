using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLibrary.Interfaces;

namespace TaskLibrary.Services
{
    public class ExtensionConstraint : IExtensionConstrains
    {
        private readonly List<string> _extensions;

        public ExtensionConstraint(string valueString)
        {
            _extensions = new List<string>();

            GetExtensions(valueString);
        }

        public void GetExtensions(string valueString)
        {
            var extension = valueString.Split(',');

            foreach (var i in extension)
            {
                _extensions.Add("." + i);
            }
        }

        public bool IsMatch(Uri uri)
        {
            var fileExtension = uri.Segments.Last();

            return _extensions.Any(e => fileExtension.EndsWith(e));
        }
    }
}
