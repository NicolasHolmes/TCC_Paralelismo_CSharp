using System;
using System.ComponentModel.DataAnnotations;

namespace Models.SQLEntities.Base.Interfaces
{
    public interface IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public void BuildCreated()
        {
            CreationDate = DateTime.Now;
        }
    }
}
