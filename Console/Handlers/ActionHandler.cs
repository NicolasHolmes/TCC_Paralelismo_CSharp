using Bot.Commands;
using Bot.Handlers.Base;
using Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtractBot.Api.Handlers
{
    public class ActionHandler : HandlerBase
    {
        private HandlerBase _handler;
        private ExtractHandler _extractHandler = new ExtractHandler();

        public override void SetHandler(HandlerBase handler)
        {
            _handler = handler;
        }

        public override async Task HandleRequestAsync(List<string> commands)
        {
            string command = commands.FirstOrDefault();
            commands.RemoveAt(0);
            bool nextHandler = false;

            if (ActionCommand.EXTRACT.Equals(command))
            {
                nextHandler = true;
                await _extractHandler.HandleRequestAsync(commands);
            }
            else if (ActionCommand.CLEAR.Equals(command))
            {
                Console.Clear();
                ConsoleExtension.WriteLog("---------ExtractBot---------");
            }
            else if (ActionCommand.HELP.Equals(command))
            {
                const string helpText = "\n\t" +
                    "Clear  - Apaga a tela\n\t" +
                    "Help   - Menu de ajuda\n\t" +
                    "Extract   - Extrair\n\t" +
                    "   Deputados(endpoint que trás informações rasas de todos os deputados de uma vez)\n\t" +
                    "Time   - Mostra a quanto tempo o Bot está rodando";
                
                ConsoleExtension.WriteSuccess(helpText);
            }
            else if (ActionCommand.TIME.Equals(command))
            {
                TimeSpan timeSpan = BotConfig.Instance.TimePass();

                ConsoleExtension.WriteLog($"{timeSpan.Hours}h{timeSpan.Minutes}m{timeSpan.Seconds}s");
            }
            else
            {
                ConsoleExtension.WriteError("Comando não reconhecido!");
            }

            if (nextHandler)
            {
                if (_handler != null)
                {
                    await _handler.HandleRequestAsync(commands);
                }
                else
                {
                    await base.HandleRequestAsync(commands);
                }
            }
        }
    }
}
