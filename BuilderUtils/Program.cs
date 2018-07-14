using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuilderUtils.Models;
using BuilderUtils.Services;
using System;
using ITGlobal.CommandLine;
using System.Reflection;
using BuilderUtils.Handlers;

namespace DrawBlipBuilderFlow
{
    public class Program
    {
        private static IServicesHub _servicesHub = new ServicesHub();
        private static ISwitch _verbose;
        private static ISwitch _force;

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                UseCLI(args);
            }
            else
            {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1- Create output hub");
                Console.Write("Answer: ");
                var answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        _servicesHub.CreateOutputHub();
                        break;
                    default:
                        break;
                }
            }
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }

        public static int UseCLI(string[] args)
        {
            return CLI.HandleErrors(() =>
            {
                var app = CLI.Parser();

                app.ExecutableName(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                app.FromAssembly(typeof(Program).GetTypeInfo().Assembly);
                app.HelpText("BLiP Builder Utils Command Line Interface");

                _verbose = app.Switch("v").Alias("verbose").HelpText("Enable verbose output.");
                _force = app.Switch("force").HelpText("Enable force operation.");

                var outputHubHandler = new OutputHubHandler();
                var outputHubCommand = app.Command("output-hub");
                outputHubHandler.Path = outputHubCommand.Parameter<string>("pth").Alias("path").Alias("pt").HelpText("Full path to the .json file (extension included)");
                outputHubHandler.StateId = outputHubCommand.Parameter<string>("sid").Alias("stateid").Alias("st").HelpText("state.id of the hub-to-be box");
                outputHubHandler.Variable = outputHubCommand.Parameter<string>("red").Alias("variable").Alias("var").HelpText("Builder variable to use on Output Condition");
                outputHubHandler.Verbose = _verbose;
                outputHubHandler.Force = _force;
                outputHubCommand.HelpText("Create a hub box connecting to every input-enabled box in the entire bot.");
                outputHubCommand.Handler(outputHubHandler.Run);

                app.HelpCommand();

                return app.Parse(args).Run();
            });
        }
    }
}
