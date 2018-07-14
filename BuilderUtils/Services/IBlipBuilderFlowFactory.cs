using BuilderUtils.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderUtils.Services
{
    public interface IBlipBuilderFlowFactory
    {
        BlipBuilderFlow Build(JObject deserializedJsonFlow);
    }
}
