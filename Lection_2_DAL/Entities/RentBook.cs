using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_DAL.Entities
{
    public class RentBook : BaseEntity
    {
        public Guid ClientId { get; set; }
        public Guid LibraryBookId { get; set; }
        public DateTime DateGet { get; set; }
        public DateTime DateReturn { get; set; }
    }
}
