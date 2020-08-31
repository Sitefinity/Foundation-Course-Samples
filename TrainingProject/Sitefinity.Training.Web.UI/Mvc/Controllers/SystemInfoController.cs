using System;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace Sitefinity.Training.Web.UI.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "SystemInfo", Title = "System Info", SectionName = "System")]
    public class SystemInfoController : Controller
    {
        public string Message { get; set; }

        public ActionResult Index()
        {
            ViewBag.OS = Environment.OSVersion;
            ViewBag.Message = this.Message;
            return View("Index");
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
        }
    }
}
