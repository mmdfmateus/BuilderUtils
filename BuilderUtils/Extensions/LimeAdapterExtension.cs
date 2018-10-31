using BuilderUtils.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderUtils.Extensions
{
    public class LimeAdapterExtension
    {
        public string QuickReplyToString(object deserializedJson)
        {
            try
            {
                var quickReply = JsonConvert.DeserializeObject<QuickReply>(deserializedJson.ToString());
                var qrString = $"{quickReply.Text}";
                foreach (var option in quickReply.Options)
                {
                    qrString += $"\n\n{option.Text}";
                }
                return qrString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
