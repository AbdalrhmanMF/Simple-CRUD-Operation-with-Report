using SimpleProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProj.ViewModels
{
    public class MoviesVM
    {
        public int Id { get; set; }
        [Required,StringLength(50)]
        public string Code { get; set; }
        [Required, StringLength(100)]
        public string ArabicName { get; set; }
        [Required, StringLength(100)]
        public string EnglishName { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; }

        public int Year { get; set; }

        [Range(1,10)]
        public double Rate { get; set; }

        [Required, StringLength(2000)]
        public string StoreLine { get; set; }

        //[Required]
        [Display(Name = "Choose Poster..")]
        public byte[] Poster { get; set; }

        [Required]
        [Display(Name ="Genre")]
        public byte GenreId { get; set; }
        
        public IEnumerable<Genre> Genres { get; set; }
    }
}
