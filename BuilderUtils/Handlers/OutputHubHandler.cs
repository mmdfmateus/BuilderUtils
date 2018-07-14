using BuilderUtils.Services;
using ITGlobal.CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuilderUtils.Handlers
{
    public class OutputHubHandler : HandlerAsync
    {
        private static IServicesHub servicesHub = new ServicesHub();
        public INamedParameter<string> StateId { get; set; }
        public INamedParameter<string> Variable { get; set; }
        public INamedParameter<string> Path { get; set; }
        public ISwitch Force { get; set; }
        public ISwitch Verbose { get; set; }
        public ISwitch Help { get; set; }
        public override int Run(string[] args)
        {
            try
            {
                if ((!StateId.IsSet || !Variable.IsSet || !Path.IsSet) && !Help.IsSet)
                    throw new ArgumentNullException("You must provide stateId, variable and path parameters for this action. Use [--stateid | --sid], [--variable| --var] and [--path | --pth] parameters");
                if (Help.IsSet)
                {
                    PrintHelp();
                    return 2;
                }
                var stateId = StateId.Value;
                var path = Path.Value;
                var variable = Variable.Value;
                VerboseServices.LogVerboseLine(Verbose.IsSet, $"Creating output hub on {stateId} using variable {variable} on flow {System.IO.Path.GetFileName(path)}");
                servicesHub.CreateOutputHub(Verbose.IsSet, stateId, variable, path);
                return 0;
            }
            catch (Exception ex)
            {
                PrintException(ex);
                return 1;
            }
        }

        private void PrintException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("\tERROR");
            Console.ResetColor();
            Console.WriteLine(ex.Message + "\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Null Parameters");
            Console.ResetColor();
            Console.WriteLine("===============\n");
            Console.ForegroundColor = ConsoleColor.Cyan;

            if (StateId.Value == null) Console.WriteLine(" " + nameof(StateId));
            if (Variable.Value == null) Console.WriteLine(" " + nameof(Variable));
            if (Path.Value == null) Console.WriteLine(" " + nameof(Path));
            Console.ResetColor();
        }

        private static void PrintHelp()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Output-Hub Params");
            Console.ResetColor();
            Console.WriteLine("===============\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  [--stateid]");
            Console.WriteLine($"  [--variable]");
            Console.WriteLine($"  [--path]");

            Console.ResetColor();
        }
    }
}
