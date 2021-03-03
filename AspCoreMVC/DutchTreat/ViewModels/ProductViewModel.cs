using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ArtDescription { get; set; }
        [Required]
        public string ArtDating { get; set; }
        [Required]
        public string ArtId { get; set; }
        [Required]
        public string Artist { get; set; }
        [Required]
        public DateTime ArtistBirthDate { get; set; }
        public DateTime ArtistDeathDate { get; set; }
        [Required]
        public string ArtistNationality { get; set; }
    }
}
