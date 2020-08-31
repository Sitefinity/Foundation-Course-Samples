using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Modules.News;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.News.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name ="Articles Widget", Title ="Articles", SectionName ="Custom")]
    public class ArticlesController : Controller
    {
        // GET: Articles
        public ActionResult Index()
        {
            return View("ArticlesList", this.GetNewsItems());
        }

        private List<NewsItem> GetNewsItems()
        {
            var newsManager = NewsManager.GetManager();

            return newsManager.GetNewsItems()
                .Where(n => n.Status == ContentLifecycleStatus.Live && n.Visible)
                .OrderByDescending(n => n.PublicationDate)
                .Take(3)
                .ToList();
        } 
    }
}