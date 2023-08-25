using Models.SQLEntities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SQLEntities
{
    [Table("DeputadosDetalhes")]
    public class DeputiesDetailEntity : BaseEntity
    {
        public int IdEndpointDeputado { get; set; }
        public string NomeCivil { get; set; }
        public string Cpf { get; set; }
        public string? Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string UfNascimento { get; set; }
        public string MunicipioNascimento { get; set; }
        public string Escolaridade { get; set; }
    }
}