using Microsoft.EntityFrameworkCore;

namespace Lection_2_DAL.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Role : BaseEntity
    {
        public string Name { get; set; }
    }
}
