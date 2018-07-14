using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuilderUtils.Handlers
{
    public abstract class HandlerAsync
    {
        public int Run(string[] args)
        {
            return RunAsync(args).GetAwaiter().GetResult();
        }

        public abstract Task<int> RunAsync(string[] args);
    }
}
