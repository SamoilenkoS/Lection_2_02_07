using Microsoft.EntityFrameworkCore;

namespace Lection_2_DAL.Entities
{
    public class Point : BaseEntity
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}
