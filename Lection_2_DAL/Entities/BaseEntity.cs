using System;
using System.ComponentModel.DataAnnotations;

namespace Lection_2_DAL.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
