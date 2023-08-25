using Infrastructure.Services.Base.Interfaces;
using Models.ExternalEntities;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IDeputyDetailService : IExtractService
    {
        public Task<DeputiesDetailResponse> GetDeputiesDetailResponseByApiAsync(int id, int requestNumber);
    }
}
