using SitefinityWebApp.Mvc.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "PropertiesWidget", Title = "Properties Widget", SectionName = "Custom")]
    public class PropertiesController : Controller
    {
        #region Accessors Options

        public string Accessors_PublicProp { get; set; }

        protected string Accessors_ProtectedProp { get; set; }

        private string Accessors_PrivateProp { get; set; }

        #endregion

        #region Display Options

        [DisplayName("Very Fancy Name Property")]
        public string Display_FancyNameProp { get; set; }

        [Browsable(false)]
        public string Display_HiddenProp { get; set; }

        #endregion

        #region Primitive Properties

        public ListChangedType Primitive_EnumProp { get; set; }

        public bool Primitive_BoolProp { get; set; }

        public int Primitive_IntProp { get; set; }

        #endregion

        #region Complex Properties

        public ComplexModel X_ComplexProp { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ComplexModel X_ComplexPropTypeConverter { get; set; }

        public List<string> X_ListProp
        {
            get
            {
                if (this._simpleList == null)
                    this._simpleList = new List<string>()
                {
                    "Item 1",
                    "Item 2",
                    "Item 3"
                };

                return this._simpleList;
            }
            set
            {
                this._simpleList = value;
            }
        }

        #endregion

        public ActionResult Index()
        {
            return Content("It's all about the Props");
        }

        private List<string> _simpleList;
    }
}