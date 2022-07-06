using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
