using Progress.Sitefinity.Renderer.Entities.Content;
using SitefinityWebApp.Custom.Helpers;
using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace SitefinityWebApp.Mvc.Models
{
	public class DemoModel
	{
		public Type OfficeType => TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Meettheteam.Office");

        public DemoModel() { }

        public List<OfficeViewModel> GetOfficesViewModel(MixedContentContext offices)
        {
            return ManagerBase.GetItems(offices, OfficeType.FullName)
                .OfType<DynamicContent>()
                .Select(o => ToViewModel(o))
                .ToList();
        }

        private OfficeViewModel ToViewModel(DynamicContent office) =>
            new OfficeViewModel
            {
                Id = office.Id,
                Title = office.GetString("Title").Value,
                Info = office.GetString("Info").Value,
                Picture = this.GetImageViewModel(office.GetRelatedItems<Image>("Picture").ToList())
            };

        private ImageViewModel GetImageViewModel(List<Image> relatedImages)
        {
            var image = new ImageViewModel();
            if (relatedImages.Any())
            {
                var relatedImage = relatedImages.First();
                image.Id = relatedImage.Id;
                image.ImageUrl = relatedImage.MediaUrl;
                image.AltText = relatedImage.AlternativeText;
            }

            return image;
        }

	}
}