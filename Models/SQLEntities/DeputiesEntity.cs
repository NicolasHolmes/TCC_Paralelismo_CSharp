using Models.SQLEntities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SQLEntities
{
    [Table("Deputados")]
    public class DeputiesEntity : BaseEntity
    {
        public int IdEndpointDeputado { get; set; }
        public string Nome { get; set; }
        public string SiglaPartido { get; set; }
        public string? SiglaUf { get; set; }

        [ForeignKey("LegislaturaId")]
        public int LegislaturaId { get; set; }
        public string Email { get; set; }
    }
}