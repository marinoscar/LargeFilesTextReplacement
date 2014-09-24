using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplaceUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            var consoleArgs = new ConsoleArguments(args);
            if (!args.Any())
            {
                Console.WriteLine("These are the valid commands");
                Console.WriteLine();
                Console.WriteLine(consoleArgs.GetHelpText());
                BeforeEnd();
                return;
            }
            var errors = consoleArgs.GetValidationErrors();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                Console.WriteLine("There are errors");
                Console.WriteLine(errors);
                Console.WriteLine();
                Console.WriteLine("These are the valid commands");
                Console.WriteLine();
                Console.WriteLine(consoleArgs.GetHelpText());
                BeforeEnd();
                return;
            }
            var fileReplacement = new FileReplace();
            fileReplacement.MessageSent += PrintFileReplaceMessage;
            Console.WriteLine("Starting replacement at {0}", DateTime.Now);
            Console.WriteLine();
            var sw = Stopwatch.StartNew();
            fileReplacement.DoReplaceInFile(consoleArgs.FileName, consoleArgs.TextToFind, consoleArgs.TextToReplace,
                                            consoleArgs.StartLine, consoleArgs.EndLine);
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("File replacement completed in {0}", sw.Elapsed);
            BeforeEnd();
        }

        static void PrintFileReplaceMessage(object sender, FileReplaceEventArgs args)
        {
            Console.Write("\r{0}", args.Message);
        }

        static void BeforeEnd()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }
    }
}
