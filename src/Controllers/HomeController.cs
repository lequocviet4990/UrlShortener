using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShortLinkApi.Models;
using ShortLinkApiCore.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkApiCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ShortenDBContext _db;

        public HomeController(ILogger<HomeController> logger, ShortenDBContext db)
        {
            _logger = logger;
            this._db = db;
        }

        public  IActionResult Index ()
        { 
            return View();
        }


        public IActionResult About()
        {
            return View();
        }



        [Route("shorten")]
        [HttpPost]
        public async Task<IActionResult> IndexAsync(string url)
        {
            string urlshorten = string.Empty;

            // Test our URL
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri result))
            {
                ViewBag.Errro = "Could not understand URL";
                return View();
            }

            var urlResult = result.ToString();

            var entry = _db.ShortLinks.Where(p => p.Url == url).FirstOrDefault();

            if (entry is null)
            {
                entry = new ShortLink
                {
                    Url = url
                };
                _db.ShortLinks.Add(entry);
                await _db.SaveChangesAsync();
            }

            var urlChunk = entry.GetUrlChunk();
            var responseUri = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host}/{urlChunk}";

            ViewBag.URLSHORTEN = responseUri;
            return View();
        }


        [Route("redirect")]
        [HttpGet]
        public async Task<IActionResult >RedirectAsync(int id)
        {
            var entry = await _db.ShortLinks.FindAsync(id);

            if (entry is not null)
            {
                ViewBag.Redirect = entry.Url;
            }
             
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}