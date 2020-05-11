using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccomodationPackagesListingViewModel
    {
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
    }
}