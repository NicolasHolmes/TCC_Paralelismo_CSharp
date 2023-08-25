using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Handlers.Base
{
    public abstract class HandlerBase
    {
        public abstract void SetHandler(HandlerBase handler);

        public virtual async Task HandleRequestAsync(List<string> commands)
        {
            
        }
    }
}
