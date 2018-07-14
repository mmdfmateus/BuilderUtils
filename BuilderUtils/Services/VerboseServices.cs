using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderUtils.Services
{
    public static class VerboseServices
    {
        public static void LogVerboseLine(bool verbose, string message)
        {
            if (verbose) Console.WriteLine(message);
        }
    }
}
