using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskLibrary.Enums;
using TaskLibrary.Interfaces;
using TaskLibrary.Services;
using Constraint = TaskLibrary.Services.Constraint;

namespace TaskConsole
{
    class Program
    {
        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private static string page = @"https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/";

        private static int depth = 2;

        private static string directoryPath = @"D:\epam-lab\site\";

        static void Main(string[] args)
        {         
            var token = cancelTokenSource.Token;

            IStore saver = new Store(directoryPath);

            IExtensionConstrains extension = new ExtensionConstraint("png,gif,jpeg,jpg,pdf");

            IConstrains constrains = new Constraint(Constraints.CurrentDomainOnly);

            IDowloader downloader = new Dowloader(saver, extension, constrains, page, depth, token);

            downloader.LoadUrl();

            Console.ReadLine();
        }
    }
}
