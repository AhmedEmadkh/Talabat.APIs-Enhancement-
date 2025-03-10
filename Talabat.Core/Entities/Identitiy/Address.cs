﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Identitiy
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AppUserId { get; set; } // FK to tell the EF that the Mandatory is the Address and the Opitonal is the AppUser
        public AppUser User { get; set; } // Navigational Property [ONE]
    }
}
