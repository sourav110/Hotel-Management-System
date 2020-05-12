using HMS.Entities;
using HMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccomodationsListingViewModel
    {
        public IEnumerable<Accomodation> Accomodations { get; set; }
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
        public int? AccomodationPackageId { get; set; }
        public string SearchTerm { get; set; }
        public Pager Pager { get; set; }
    }
}