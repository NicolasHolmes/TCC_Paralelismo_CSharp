using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.Base.Interfaces
{
    public interface IExtractService : IService
    {
        public Task ProcessAsync();
    }
}
