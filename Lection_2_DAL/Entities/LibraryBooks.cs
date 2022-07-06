using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_DAL.Entities
{
    public class LibraryBooks : BaseEntity
    {
        public Guid RevisionId { get; set; }
        public BookRevision BookRevision { get; set; }
        public Guid LibraryId { get; set; }
        public Library Library { get; set; }
    }
}
