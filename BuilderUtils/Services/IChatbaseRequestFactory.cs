using System;
using System.Collections.Generic;
using System.Text;
using BuilderUtils.Models;
using Newtonsoft.Json.Linq;

namespace BuilderUtils.Services
{
    public interface IChatbaseRequestFactory
    {
        ChatbaseRequest Build(JObject deserializedJsonFlow);
    }
}
