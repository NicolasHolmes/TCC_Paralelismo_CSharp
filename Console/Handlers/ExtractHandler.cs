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

        private IProductService _productService;
        private IProductDetailService _productDetailService;
        public ExtractHandler()
        {
            _startup = new Startup();

            _productService = _startup.Provider.GetRequiredService<IProductService>();
            _productDetailService = _startup.Provider.GetRequiredService<IProductDetailService>();

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
                ConsoleExtension.WriteLog("\t Lista de Produtos ou Detalhes dos Produtos");
                commands = Console.ReadLine().ToCommands();
            }
            bool nextHandler = false;
            bool tryAgain = false;
            do
            {
                if (ExtractCommand.PRODUTOS.Equals(command))
                {
                    _extractService = _productService;
                }
                else if (ExtractCommand.DETALHESPRODUTOS.Equals(command))
                {
                    _extractService = _productDetailService;
                }
                else
                {
                    ConsoleExtension.WriteError("Comando não reconhecido!");
                    ConsoleExtension.WriteLog("\t Lista de Produtos ou Detalhes dos Produtos?");
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
