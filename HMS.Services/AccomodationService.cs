using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccomodationService
    {
        public IEnumerable<Accomodation> GetAllAccomodations()
        {
            var _context = new HMSContext();
            return _context.Accomodations.ToList();
        }

        public Accomodation GetAccomodationById(int id)
        {
            using (var _context = new HMSContext())
            {
                return _context.Accomodations.Find(id);
            }
        }

        public IEnumerable<Accomodation> SearchAccomodations(string searchTerm, int? accomodationPackageId, int pageNo, int recordSize)
        {
            var _context = new HMSContext();
            var accomodations = _context.Accomodations.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodations = accomodations.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (accomodationPackageId.HasValue && accomodationPackageId.Value > 0)
            {
                accomodations = accomodations.Where(a => a.AccomodationPackageId == accomodationPackageId.Value);
            }

            var skip = (pageNo - 1) * recordSize;

            return accomodations.OrderBy(x => x.AccomodationPackageId).Skip(skip).Take(recordSize).ToList();
        }

        public int SearchAccomodationsCount(string searchTerm, int? accomodationPackageId)
        {
            var _context = new HMSContext();
            var accomodations = _context.Accomodations.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodations = accomodations.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (accomodationPackageId.HasValue && accomodationPackageId.Value > 0)
            {
                accomodations = accomodations.Where(a => a.AccomodationPackageId == accomodationPackageId.Value);
            }

            return accomodations.Count();
        }

        public bool SaveAccomodation(Accomodation accomodation)
        {
            var _context = new HMSContext();
            _context.Accomodations.Add(accomodation);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateAccomodation(Accomodation accomodation)
        {
            var _context = new HMSContext();
            _context.Entry(accomodation).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public bool DeleteAccomodation(Accomodation accomodation)
        {
            var _context = new HMSContext();
            _context.Entry(accomodation).State = System.Data.Entity.EntityState.Deleted;
            return _context.SaveChanges() > 0;
        }
    }
}
