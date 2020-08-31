using SitefinityWebApp.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "FlatTaxonomies", Title ="Flat Taxonomies", SectionName = "Classifications")]
    public class FlatTaxonomyController : Controller
    {
        // GET: FlatTaxonomy
        public ActionResult Index()
        {
            var model = new FlatTaxonomyModel();
            model.Populate();
            return View("Index", model);
        }
    }
}