using Dapper;
using Models.SQLEntities.Base.Interfaces;
using System;

namespace Models.SQLEntities.Base
{
    public abstract class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            DataCriacao = DateTime.Now;
            DataAtualizacao = DateTime.Now;
            Deletado = false;
        }
        [ReadOnly(true)]
        [Key]
        public int Id { get; set; }
        [IgnoreUpdate]
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Deletado { get; set; }

        public BaseEntity BuildCreated()
        {
            DataCriacao = DateTime.Now;
            DataAtualizacao = null;
            Deletado = false;
            return this;
        }
        public BaseEntity BuildUpdated()
        {
            DataAtualizacao = DateTime.Now;
            return this;
        }
        public BaseEntity BuildDeleted()
        {
            Deletado = true;
            return this;
        }
        public BaseEntity BuildNotDeleted()
        {
            Deletado = false;
            return this;
        }
    }
}
