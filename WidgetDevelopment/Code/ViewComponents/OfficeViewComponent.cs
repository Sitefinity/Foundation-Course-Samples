using Microsoft.AspNetCore.Mvc;
using Progress.Sitefinity.AspNetCore.ViewComponents;
using Renderer.Entities.Office;
using Renderer.Models.Office;
using System.Threading.Tasks;

namespace Renderer.ViewComponents
{
    [SitefinityWidget]
    public class OfficeViewComponent : ViewComponent
    {
        private readonly IOfficeModel officeModel;

        public OfficeViewComponent( IOfficeModel _officeModel ) =>
            officeModel = _officeModel;

        public async Task<IViewComponentResult> InvokeAsync( IViewComponentContext<OfficeEntity> context ) =>
            View( context.Entity.ViewName, await officeModel.GetViewModels( context.Entity ) );
    }
}
