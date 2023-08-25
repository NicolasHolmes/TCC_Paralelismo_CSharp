using System;
using System.Collections.Generic;

namespace Models.ExternalEntities
{
    public class DeputiesDetailResponse
    {
        public DeputiesDetailData dados { get; set; }
        public List<Link> links { get; set; }
    }

    public class DeputiesDetailData
    {
        public int id { get; set; }
        public string uri { get; set; }
        public string nomeCivil { get; set; }
        public DeputiesDetailStatus ultimoStatus { get; set; }
        public string cpf { get; set; }
        public string sexo { get; set; }
        public string urlWebsite { get; set; }
        public List<string> redeSocial { get; set; }
        public DateTime dataNascimento { get; set; }
        public string ufNascimento { get; set; }
        public string municipioNascimento { get; set; }
        public string escolaridade { get; set; }
    }

    public class DeputiesDetailStatus
    {
        public int id { get; set; }
        public string uri { get; set; }
        public string nome { get; set; }
        public string siglaPartido { get; set; }
        public string uriPartido { get; set; }
        public string siglaUf { get; set; }
        public int idLegislatura { get; set; }
        public string urlFoto { get; set; }
        public string email { get; set; }
        public DateTime data { get; set; }
        public string nomeEleitoral { get; set; }
        public DeputiesDetailGabinete gabinete { get; set; }
        public string situacao { get; set; }
        public string condicaoEleitoral { get; set; }
        public string descricaoStatus { get; set; }
    }

    public class DeputiesDetailGabinete
    {
        public string nome { get; set; }
        public string predio { get; set; }
        public string sala { get; set; }
        public string andar { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }
}
