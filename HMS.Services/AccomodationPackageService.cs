using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccomodationPackageService
    {
        public IEnumerable<AccomodationPackage> GetAllAccomodationPackages()
        {
            var _context = new HMSContext();
            return _context.AccomodationPackages.ToList();
        }

        public AccomodationPackage GetAccomodationPackageById(int id)
        {
            using (var _context = new HMSContext())
            {
                return _context.AccomodationPackages.Find(id);
            }
        }

        public IEnumerable<AccomodationPackage> SearchAccomodationPackages(string searchTerm, int? accomodationTypeId, int pageNo, int recordSize)
        {
            var _context = new HMSContext();
            var accomodationPackages = _context.AccomodationPackages.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackages = accomodationPackages.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (accomodationTypeId.HasValue && accomodationTypeId.Value > 0)
            {
                accomodationPackages = accomodationPackages.Where(a => a.AccomodationTypeId == accomodationTypeId.Value);
            }

            var skip = (pageNo - 1) * recordSize;

            return accomodationPackages.OrderBy(x => x.AccomodationTypeId).Skip(skip).Take(recordSize).ToList();
        }

        public int SearchAccomodationPackagesCount(string searchTerm, int? accomodationTypeId)
        {
            var _context = new HMSContext();
            var accomodationPackages = _context.AccomodationPackages.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackages = accomodationPackages.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (accomodationTypeId.HasValue && accomodationTypeId.Value > 0)
            {
                accomodationPackages = accomodationPackages.Where(a => a.AccomodationTypeId == accomodationTypeId.Value);
            }

            return accomodationPackages.Count();
        }

        public bool SaveAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var _context = new HMSContext();
            _context.AccomodationPackages.Add(accomodationPackage);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var _context = new HMSContext();
            _context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public bool DeleteAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var _context = new HMSContext();
            _context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Deleted;
            return _context.SaveChanges() > 0;
        }
    }
}
