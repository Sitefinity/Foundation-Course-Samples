using SitefinityWebApp.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "OurOfficesWidget", Title = "Our Offices", SectionName = "Custom")]
    public class OurOfficesController : Controller
    {
        private readonly OfficeModel officeModel;

        public OurOfficesController()
        {
            this.officeModel = new OfficeModel();
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public OfficeModel OfficeModel
        {
            get
            {
                return this.officeModel;
            }
        }

        public string TemplateName { get; set; } = "Index";

        public ActionResult Index()
        {
            var viewModel = this.officeModel.GetOfficesViewModel();
            return View(this.TemplateName, viewModel);
        }

        [HttpPost]
        public ActionResult CreateOffice()
        {
            ViewBag.Result = this.officeModel.CreateOffice();
            return View("OfficeResult");
        }
    }
}