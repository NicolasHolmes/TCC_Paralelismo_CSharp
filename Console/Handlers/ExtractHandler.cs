using Bot.Commands;
using Bot.Handlers.Base;
using Infrastructure.Extensions;
using Infrastructure.Services.Base.Interfaces;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtractBot.Api.Handlers
{
    public class ExtractHandler : HandlerBase
    {
        private HandlerBase _handler;
        private Startup _startup;
        private IExtractService _extractService;

        private IDeputyService _deputyService;
        private IDeputyDetailService _deputyDetailService;

        public ExtractHandler()
        {
            _startup = new Startup();

            _deputyService = _startup.Provider.GetRequiredService<IDeputyService>();
            _deputyDetailService = _startup.Provider.GetRequiredService<IDeputyDetailService>();

        }

        public override void SetHandler(HandlerBase handler)
        {
            _handler = handler;
        }

        public override async Task HandleRequestAsync(List<string> commands)
        {
            string command = commands.FirstOrDefault();
            if (command == null)
            {
                ConsoleExtension.WriteLog("Qual endpoint deseja extrair?");
                ConsoleExtension.WriteLog("\t Lista de Deputados ou Detalhes dos Deputados");
                commands = Console.ReadLine().ToCommands();
            }
            bool nextHandler = false;
            bool tryAgain = false;
            do
            {
                if (ExtractCommand.DEPUTADOS.Equals(command))
                {
                    _extractService = _deputyService;
                }
                else if (ExtractCommand.DETALHESDEPUTADOS.Equals(command))
                {
                    _extractService = _deputyDetailService;
                }
                else
                {
                    ConsoleExtension.WriteError("Comando não reconhecido!");
                    ConsoleExtension.WriteLog("\t Lista de Deputados ou Detalhes dos Deputados?");
                    commands = Console.ReadLine().ToCommands();
                    tryAgain = true;
                }
            } while (tryAgain);

            //ConsoleExtension.WriteLog("Por quantas horas?");
            //DateTime timeLimit = DateTime.Now.AddHours(int.Parse(Console.ReadLine()));
            //ConsoleExtension.WriteLog("E quantos minutos?");
            //timeLimit.AddMinutes(int.Parse(Console.ReadLine()));

            await _extractService.ProcessAsync();

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
