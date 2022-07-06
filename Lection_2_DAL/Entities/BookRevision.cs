using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_DAL.Entities
{
    public class BookRevision : BaseEntity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public int YearOfPublishing { get; set; }
        public int PagesCount { get; set; }
        public float Price { get; set; }
    }
}
