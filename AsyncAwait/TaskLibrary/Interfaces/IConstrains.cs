using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLibrary.Interfaces
{
    public interface IConstrains
    {
        bool IsMatch(Uri currentUri, Uri secondUri);
    }
}
