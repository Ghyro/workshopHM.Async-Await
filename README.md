# workshopHM.Async-Await

## Task
It is necessary to implement the library and the console program using it to create a local copy of the site (“analogue” of the [wget](https://ru.wikipedia.org/wiki/Wget) program).
Working with the program looks like this: the user specifies the starting point (URL) and the folder where to save, and the program goes through all available links and recursively downloads the site (s).

##### Program / Library Options:

 1. Restriction on the depth of link analysis (i.e. if you downloaded the page that the user specified, it is level 0, all pages to which will enter links from it, it is level 1, etc.)
  
 2. Restriction on switching to other domains (no restrictions / only within the current domain / not higher than the path in the source URL)
  
 3. Restriction on the “expansion” of downloaded resources (you can set a list, for example: gif, jpeg, jpg, pdf)
  
##### Recommendations for implementation
As a basis, you can take the following libraries:
 1. Work with HTTP 
   - System.Net.Http.HttpClient - recommended option
   - If you are working with .Net 4.5 +, it is included in the framework itself. In earlier versions and for other platforms, get through [NuGet](https://www.nuget.org/packages/Microsoft.Net.Http)
   - An introduction to working with him can be found here https://blogs.msdn.microsoft.com/henrikn/2012/02/16/httpclient-is-here/
   - Please note - it is all built on asynchronous operations
   - System.Net.HttpWebRequest - legacy

 2. Working with HTML
   - You can use one of the libraries listed [here](https://ru.stackoverflow.com/questions/420354/%D0%9A%D0%B0%D0%BA-%D1%80%D0%B0%D1%81%D0%BF%D0%B0%D1%80%D1%81%D0%B8%D1%82%D1%8C-html-%D0%B2-net/450586).
