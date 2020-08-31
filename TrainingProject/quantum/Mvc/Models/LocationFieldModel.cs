using Telerik.Sitefinity.Frontend.Forms.Mvc.Models;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Metadata.Model;
using Telerik.Sitefinity.Modules.Forms.Web.UI;

namespace SitefinityWebApp.Mvc.Models
{
    public class LocationFieldModel : FormFieldModel, IHideable
    {
        public ZoomLevel Zoom { get; set; }

        bool IHideable.Hidden { get; set; }

        public override object GetViewModel(object value, IMetaField metaField)
        {
            this.Value = value as string ?? this.MetaField.DefaultValue ?? string.Empty;
            return this;
        }
    }
}