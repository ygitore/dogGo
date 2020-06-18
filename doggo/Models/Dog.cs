using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doggo.Models
{
    public class Dog
    {
        public int Id;
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public string Breed { get; set; }
        public string Notes { get; set; }
        public string ImageUrl { get; set; }

    }
}
