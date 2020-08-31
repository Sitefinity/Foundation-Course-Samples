using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Mvc.Models
{
    public class TaxaCount
    {
        public Guid TaxonomyId { get; set; }
        public int Count { get; set; }
    }
}