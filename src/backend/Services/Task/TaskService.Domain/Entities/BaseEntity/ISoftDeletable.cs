using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Domain.Entities.BaseEntity
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
