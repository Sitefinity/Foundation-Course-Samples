using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Linq;
using System.Linq.Dynamic;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Multisite;
using Telerik.Sitefinity.Utilities.TypeConverters;
using System.Collections.Generic;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.DynamicModules.Builder;
using ServiceStack.Text;
using Telerik.Sitefinity.Versioning;
using System.Threading;
using System.Globalization;
using Telerik.Sitefinity.GeoLocations.Model;
using Telerik.Sitefinity.Locations;
using Telerik.Sitefinity.Locations.Configuration;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Data;

namespace SitefinityWebApp.Mvc.Models
{
    public class OfficeModel
    {
        private List<DynamicContent> officesQuery;
        public Type OfficeType => TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Meettheteam.Office");

        public string ProviderName { get; set; }
        public string StringifiedIds { get; set; }

        public OfficeModel()
        {
            if (this.ProviderName == null)
            {
                var dynamicType = ModuleBuilderManager.GetActiveTypes().FirstOrDefault(t => t.FullTypeName == this.OfficeType.FullName);
                this.ProviderName = DynamicModuleManager.GetDefaultProviderName(dynamicType.ModuleName);
            }
        }

        protected DynamicModuleManager GetManager()
        {
            return DynamicModuleManager.GetManager(this.ProviderName);
        }

        public List<OfficeViewModel> GetOfficesViewModel()
        {
            this.officesQuery = this.GetManager()
                .GetDataItems(OfficeType)
                .Where(o => o.Status == ContentLifecycleStatus.Live && o.Visible)
                .ToList();

            if (!string.IsNullOrEmpty(this.StringifiedIds))
                this.FilterOfficesByIds();

            this.officesQuery.SetRelatedDataSourceContext();

            return this.officesQuery.Select(o => ToViewModel(o))
                .OrderBy(i => i.Title)
                .ToList();
        }

        private void FilterOfficesByIds()
        {
            var selectedOfficesIds = JsonSerializer.DeserializeFromString<List<Guid>>(this.StringifiedIds);

            if (selectedOfficesIds.Any() && selectedOfficesIds.First() != Guid.Empty)
                this.officesQuery = this.officesQuery
                    .Where(o => selectedOfficesIds.Contains(o.OriginalContentId))
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

        public string CreateOffice()
        {
            string result = "Office created successfully";
            try
            {
                // Set a transaction name and get the version manager
                var transactionName = "someTransactionName";
                var versionManager = VersionManager.GetManager(null, transactionName);

                // Set the culture name for the multilingual fields
                var cultureName = "en";
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

                DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(this.ProviderName, transactionName);

                DynamicContent officeItem = dynamicModuleManager.CreateDataItem(this.OfficeType);

                // This is how values for the properties are set
                officeItem.SetString("Title", "New York", cultureName);
                officeItem.SetString("Info", LOREM_IPSUM, cultureName);
                Address address = new Address();
                CountryElement addressCountry = Telerik.Sitefinity.Configuration.Config.Get<LocationsConfig>().Countries.Values.First(x => x.Name == "United States");
                address.CountryCode = addressCountry.IsoCode;
                address.StateCode = addressCountry.StatesProvinces.Values.First().Abbreviation;
                address.City = "New York City";
                address.Street = "Baker Street";
                address.Zip = "12345";
                officeItem.SetValue("Address", address);


                // Get related item manager
                LibrariesManager pictureManager = LibrariesManager.GetManager("OpenAccessDataProvider");
                var pictureItem = pictureManager.GetImages().FirstOrDefault(i => i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Master && i.Title.Contains("New York"));
                if (pictureItem != null)
                {
                    // This is how we relate an item
                    officeItem.CreateRelation(pictureItem, "Picture");
                }

                officeItem.SetString("UrlName", "new-york", cultureName);
                officeItem.SetValue("Owner", SecurityManager.GetCurrentUserId());
                officeItem.SetValue("PublicationDate", DateTime.UtcNow);


                officeItem.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Draft", new CultureInfo(cultureName));

                // Create a version and commit the transaction in order changes to be persisted to data store
                versionManager.CreateVersion(officeItem, false);
                TransactionManager.CommitTransaction(transactionName);
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }

            return result;
        }

        public const string LOREM_IPSUM = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        public const string OFFICE_MODULE_NAME = "Meet the team";
    }
}