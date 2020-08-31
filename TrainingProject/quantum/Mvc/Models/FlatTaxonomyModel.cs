using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;

namespace SitefinityWebApp.Mvc.Models
{
    public class FlatTaxonomyModel
    {
        private readonly TaxonomyManager taxonomyManager;
        private List<TaxaCount> taxaCounts = new List<TaxaCount>();

        public FlatTaxonomyModel()
        {
            this.taxonomyManager = TaxonomyManager.GetManager();

            this.taxaCounts = this.taxonomyManager
                .GetTaxa<FlatTaxon>()
                .GroupBy(t => t.TaxonomyId)
                .Select(g => new TaxaCount() { TaxonomyId = g.Key, Count = g.Count() })
                .ToList();
        }
        public List<FlatTaxonomyViewModel> Taxonomies
        {
            private set;
            get;
        }

        public void Populate()
        {
            this.Taxonomies = this.taxonomyManager.GetTaxonomies<FlatTaxonomy>()
                .Select(t => ToViewModel(t))
                .ToList();
        }

        private FlatTaxonomyViewModel ToViewModel(FlatTaxonomy taxonomy)
        {
            var viewModel = new FlatTaxonomyViewModel();
            viewModel.Id = taxonomy.Id;
            viewModel.Name = taxonomy.Title.Value;

            var taxaCount = this.taxaCounts
                .FirstOrDefault(t => t.TaxonomyId == taxonomy.Id);

            viewModel.TaxaCount = taxaCount != null ? taxaCount.Count : 0;

            return viewModel;
            
        }
    }
}