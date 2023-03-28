using Progress.Sitefinity.AspNetCore.RestSdk;
using Progress.Sitefinity.RestSdk;
using Progress.Sitefinity.RestSdk.Client;
using Renderer.Dto;
using Renderer.Entities.Office;
using Renderer.ViewModels.Office;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.Office
{
    public class OfficeModel : IOfficeModel
    {
        private readonly IRestClient client;

        public OfficeModel( IRestClient client ) =>
            this.client = client;

        public async Task<List<OfficeViewModel>> GetViewModels( OfficeEntity entity )
        {
            var args = new GetAllArgs()
            {
                Fields = new string [] { "Id", "Picture", "Title", "Info" }
            };
            CollectionResponse<OfficeItem> response = await client.GetItems<OfficeItem>( entity.Offices, args )
                .ConfigureAwait( true );

            return response.Items
                .Select( x => GetItemViewModel( x ) ).ToList();
        }

        private static OfficeViewModel GetItemViewModel( OfficeItem item )
        {
            var viewModel = new OfficeViewModel()
            {
                Title = item.Title,
                Info = item.Info
            };

            if ( item.Picture != null && item.Picture.Any() )
            {
                viewModel.ImageUrl = item.Picture[0].ThumbnailUrl;
                viewModel.AltText = item.Picture[0].AlternativeText;
            }

            return viewModel;
        }
    }
}
