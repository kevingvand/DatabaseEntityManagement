using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Entities._Base
{
    public class BaseEntity
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool Deprecated { get; set; }

        public BaseEntity() { }

        public BaseEntity(BaseEntity entity)
        {
            Id = entity.Id;
            CreationDate = entity.CreationDate;
            ModifiedDate = entity.ModifiedDate;
        }
    }
}
