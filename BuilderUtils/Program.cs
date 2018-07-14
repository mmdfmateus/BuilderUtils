using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuilderUtils.Models;
using BuilderUtils.Services;
using System;

namespace DrawBlipBuilderFlow
{
    public class Program
    {
        private static IServicesHub _servicesHub = new ServicesHub();

        static void Main(string[] args)
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
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
