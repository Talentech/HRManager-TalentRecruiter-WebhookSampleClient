using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace HRMTS.WebhookClient.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var postData = PostData.ReadFromDisk();

            var output = string.Empty;

            var list = postData.Items;

            list.Reverse();

            ViewBag.Output = list;

            return View();
        }

        [HttpPost]
        public HttpStatusCode Post()
        {
            var data = string.Empty;

            var memoryStream = new MemoryStream();

            Request.Body.CopyTo(memoryStream);

            data = Encoding.UTF8.GetString(memoryStream.ToArray());

            if (!string.IsNullOrWhiteSpace(data))
            {
                var postData = PostData.ReadFromDisk();

                var headers = new List<KeyValue>();

                foreach (var header in Request.Headers.ToList())
                    headers.Add(new KeyValue(header.Key, header.Value));

                postData.Items.Add(new PostItem {Body = data, Headers = headers});

                PostData.WriteToDisk(postData);
            }

            return HttpStatusCode.OK;
        }
    }
}