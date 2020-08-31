using System.Collections.Generic;
using System.ComponentModel;
using SitefinityWebApp.Mvc.Models;
using Telerik.Sitefinity.Data.Metadata;
using Telerik.Sitefinity.Forms.Model;
using Telerik.Sitefinity.Frontend.Forms;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.TextField;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Modules.Pages.Web.Services;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Web.UI.Fields.Enums;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "LocationField", Title = "Location Field", Toolbox = FormsConstants.FormControlsToolboxName, SectionName = FormsConstants.CommonSectionName)]
    [DatabaseMapping(UserFriendlyDataType.ShortText)]
    public class LocationFieldController : FormFieldControllerBase<LocationFieldModel>, ISupportRules, ITextField
    {
        private LocationFieldModel model;

        public LocationFieldController()
        {
            this.DisplayMode = FieldDisplayMode.Write;
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [ReflectInheritedProperties]
        public override LocationFieldModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = new LocationFieldModel();

                this.model.Zoom = ZoomLevel.City;
                return this.model;
            }
        }

        public TextType InputType
        {
            get
            {
                return TextType.Text;
            }
        }

        IDictionary<ConditionOperator, string> ISupportRules.Operators
        {
            get
            {
                return new Dictionary<ConditionOperator, string>()
                {
                    [ConditionOperator.Equal] = "equal",
                    [ConditionOperator.NotEqual] = "not equal"
                };
            }
        }

        string ISupportRules.Title
        {
            get
            {
                return "Location Field";
            }
        }
    }
}