using System.Collections.Generic;

namespace Models.ExternalEntities
{
    public class DeputiesResponse
    {
        public List<DeputiesList> dados { get; set; }
    }
    public class DeputiesList
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string siglaPartido { get; set; }
        public string siglaUf { get; set; }
        public int idLegislatura { get; set; }
        public string email { get; set; }
    }
}
