using Dapper;
using Models.SQLEntities.Base.Interfaces;
using System;

namespace Models.SQLEntities.Base
{
    public abstract class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            CreationDate = DateTime.Now;
        }
        [ReadOnly(true)]
        [Key]
        public int Id { get; set; }
        [IgnoreUpdate]
        public DateTime CreationDate { get; set; }

        public BaseEntity BuildCreated()
        {
            CreationDate = DateTime.Now;
            return this;
        }
    }
}
