using Renderer.Entities.Office;
using Renderer.ViewModels.Office;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Renderer.Models.Office
{
    public interface IOfficeModel
    {
        Task<List<OfficeViewModel>> GetViewModels( OfficeEntity entity );
    }
}
