using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccomodationTypeService
    {
        
        public IEnumerable<AccomodationType> GetAllAccomodationTypes()
        {
            var _context = new HMSContext();
            return _context.AccomodationTypes.ToList();
        }

        public AccomodationType GetAccomodationTypeById(int id) 
        {
            var _context = new HMSContext();
            return _context.AccomodationTypes.Find(id);
        }

        public IEnumerable<AccomodationType> SearchAccomodationTypes(string searchTerm)
        {
            var _context = new HMSContext();
            var accomodationTypes = _context.AccomodationTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationTypes = accomodationTypes.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            return accomodationTypes.ToList();
        }

        public bool SaveAccomodationType(AccomodationType accomodationType)
        {
            var _context = new HMSContext();
            _context.AccomodationTypes.Add(accomodationType);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateAccomodationType(AccomodationType accomodationType)
        {
            var _context = new HMSContext();
            _context.Entry(accomodationType).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public bool DeleteAccomodationType(AccomodationType accomodationType)
        {
            var _context = new HMSContext();
            _context.Entry(accomodationType).State = System.Data.Entity.EntityState.Deleted;
            return _context.SaveChanges() > 0;
        }
    }
}
