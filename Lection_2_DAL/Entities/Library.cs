using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lection_2_DAL.Entities
{
    public class Library : BaseEntity
    {
        [ForeignKey("Point")]
        public Guid LocationId { get; set; }
        [ForeignKey("City")]
        public Guid CityId { get; set; }
        public City City { get; set; }
        public Point Location { get; set; }
        public string FullAddress { get; set; }
    }
}
