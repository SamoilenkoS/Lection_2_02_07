using Lection_2_DAL.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lection_2_DAL.Entities
{
    public class Book : BaseEntity
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Title { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Author { get; set; }
        public int PagesCount { get; set; }
    }
}
