using BuilderUtils.Models;
using Newtonsoft.Json;
using System;
using Lime.Messaging.Contents;
using System.Collections.Generic;
using System.Text;

namespace BuilderUtils.Extensions
{
    public class LimeAdapterExtension : ILimeAdapterExtension
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

        public string DocumentCollectionToString(object deserializedJson)
        {
            try
            {
                var carroussel = JsonConvert.DeserializeObject<DocumentCollection>(deserializedJson.ToString());
                var carrousselString = "";
                foreach (var item in carroussel.Items)
                {
                    foreach (var option in item.Options)
                    {
                        if (option.Label.Type == WebLink.MIME_TYPE)
                        {
                            var weblink = JsonConvert.DeserializeObject<Weblink>(option.Label.Value.ToString());
                            carrousselString += $"{weblink.Text}";
                        }
                        else
                        {
                            carrousselString += $"{option.Label.Value}";
                        }
                        if (item.Options.IndexOf(option) != item.Options.Count - 1)
                            carrousselString += "\n";
                    }
                    if (carroussel.Items.IndexOf(item) != carroussel.Items.Count - 1)
                        carrousselString += "\n\n";
                }
                return carrousselString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public string MediaLinkToString(object deserializedJson)
        {
            try
            {
                var mediaLink = JsonConvert.DeserializeObject<Medialink>(deserializedJson.ToString());
                var medialinkString = $"*MediaLink*\n{mediaLink.Title} ({mediaLink.Uri})";

                return medialinkString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public string LocationInputToString(object deserializedJson)
        {
            try
            {
                var inputLocation = JsonConvert.DeserializeObject<InputLocation>(deserializedJson.ToString());
                var medialinkString = $"*InputLocation*\n{inputLocation.Label.Value}";

                return medialinkString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

    }
}
