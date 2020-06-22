using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace doggo.Models
{
    public class Dog
    {
        public int Id;
        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Name is requried")]
        public string Name { get; set; }

        [DisplayName("Owner")]
        [Required]
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public string Breed { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "notes requried")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string ImageUrl { get; set; }

    }
}
