using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lection_2_DAL.Entities
{
    public class LibraryBook : BaseEntity
    {
        [ForeignKey("BookRevision")]
        public Guid RevisionId { get; set; }
        public BookRevision BookRevision { get; set; }
        [ForeignKey("Library")]
        public Guid LibraryId { get; set; }
        public Library Library { get; set; }
    }
}
