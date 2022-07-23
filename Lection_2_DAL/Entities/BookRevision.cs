using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lection_2_DAL.Entities
{
    public class BookRevision : BaseEntity
    {
        [ForeignKey("Book")]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public int YearOfPublishing { get; set; }
        public int PagesCount { get; set; }
        public float Price { get; set; }
    }
}
