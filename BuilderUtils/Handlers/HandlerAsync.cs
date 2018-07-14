using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuilderUtils.Handlers
{
    public abstract class HandlerAsync
    {
        public abstract int Run(string[] args);
    }
}
