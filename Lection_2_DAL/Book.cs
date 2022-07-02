using System;
using System.ComponentModel.DataAnnotations;

namespace Lection_2_DAL
{
    public class Book
    {
        public Guid Id { get; set; }
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
