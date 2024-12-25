using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToursApp.Entities
{
    public partial class IS24_USER10Entities 
    {
        private static IS24_USER10Entities _context;

        public static IS24_USER10Entities GetContext()
        { 
            if (_context == null) { 
                _context = new IS24_USER10Entities();
            }
            return _context; 
        }
    }
}
