using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doggo.Models
{
    public class Owner
    {
        public int Id;
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int NeighborhoodId { get; set; }
        public Neighborhood Neighborhood { get; set; }
    }
}
