using System;

namespace SitefinityWebApp.Mvc.ViewModels
{
    public class ImageViewModel
    {
        public Guid Id { get; set; }

        public string ProviderName { get; set; }

        public string ImageUrl { get; set; }

        public string AltText { get; set; }
    }
}