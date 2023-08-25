using ExtractBot.Api.Handlers;
using Infrastructure.Extensions;
using System;
using System.Threading.Tasks;

namespace ExtractBot.Api
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ConsoleExtension.WriteLog("---------ExtractBot---------");
            ConsoleKeyInfo pressedKey;
            string commandText = string.Empty;

            ActionHandler actionHandler = new ActionHandler();

            try
            {
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    pressedKey = Console.ReadKey();

                    if (pressedKey.Key == ConsoleKey.Backspace)
                    {
                        int lastIndex = commandText.Length - 1;
                        if (lastIndex > 0)
                        {
                            commandText = commandText.Remove(lastIndex - 1);
                            ConsoleExtension.Backspace(commandText);
                        }
                    }
                    else if (pressedKey.Key == ConsoleKey.Enter)
                    {
                        commandText = commandText.TrimStart().ToLower();
                        ConsoleExtension.ClearCurrentConsoleLine();
                        ConsoleExtension.WriteCommandMessage(commandText);

                        await actionHandler.HandleRequestAsync(commandText.ToCommands());

                        commandText = string.Empty;
                    }
                    else if (pressedKey.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else
                    {
                        commandText += pressedKey.KeyChar;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"o erro é: '{e}'");
            }
        }
    }
}
