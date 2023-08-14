using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProj.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [Required,MaxLength(50)]
        public string Code { get; set; }
        [Required,MaxLength(100)]
        public string ArabicName { get; set; }
        [Required,MaxLength(100)]
        public string EnglishName { get; set; }
    }
}
