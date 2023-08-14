using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProj.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,MaxLength(50)]
        public string Code { get; set; }
        [Required,MaxLength(100)]
        public string ArabicName { get; set; }
        [Required,MaxLength(100)]
        public string EnglishName { get; set; }

        [Required,MaxLength(50)]
        public string Title { get; set; }

        public int Year { get; set; }
        public double Rate { get; set; }

        [Required,MaxLength(2000)]
        public string StoreLine { get; set; }

        [Required]
        public byte[] Poster { get; set; }

        [Required]
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }
    }

}
