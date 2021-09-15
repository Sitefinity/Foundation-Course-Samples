using Progress.Sitefinity.AspNetCore.ViewComponents.AttributeConfigurator.Attributes;
using Progress.Sitefinity.Renderer.Designers.Attributes;
using Progress.Sitefinity.Renderer.Entities.Content;
using System.ComponentModel;

namespace Renderer.Entities.Office
{
    public class OfficeEntity
    {
        [Content( Type = "Telerik.Sitefinity.DynamicTypes.Model.Meettheteam.Office" )]
        public MixedContentContext Offices { get; set; }

        [ViewSelector]
        [DefaultValue( "Grid" )]
        public string ViewName { get; set; }
    }
}
