﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace BuilderUtils.Models
{
    public partial class BlipBuilderFlow
    {
        //[JsonProperty("onboarding")]
        public BlipBuilderFlow()
        {
            Boxes = new List<Box>();
            Proxy = new List<BoxProxy>();
        }
        public List<Box> Boxes { get; set; }
        public List<BoxProxy> Proxy { get; set; }

        public BlipBuilderFlow ParseProxyIntoFlow()
        {
            if (Proxy == null || Proxy.Count == 0) throw new Exception("Proxy list empty or null");
            Boxes = new List<Box>();
            foreach (var item in Proxy)
            {
                var box = new Box(item);
                Boxes.Add(box);
            }
            return this;
        }
    }

    public class BoxProxy
    {
        public string Key { get; set; }
        public BoxContent Content { get; set; }
    }

    public partial class Box
    {
        public Box(BoxProxy proxy)
        {
            Content = new Dictionary<string, BoxContent>();
            Content.Add(proxy.Key, proxy.Content);
        }
        public Dictionary<string, BoxContent> Content { get; set; }
    }

    public class BoxContent
    {
        [JsonProperty("$contentActions")]
        public List<ContentAction> ContentActions { get; set; }

        [JsonProperty("$conditionOutputs")]
        public List<ConditionOutput> ConditionOutputs { get; set; }

        [JsonProperty("$enteringCustomActions")]
        public List<CustomAction> EnteringCustomActions { get; set; }

        [JsonProperty("$leavingCustomActions")]
        public List<CustomAction> LeavingCustomActions { get; set; }

        [JsonProperty("$defaultOutput")]
        public DefaultOutput DefaultOutput { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("root")]
        public bool Root { get; set; }

        [JsonProperty("$position")]
        public Position Position { get; set; }

        [JsonProperty("$title")]
        public string Title { get; set; }

        [JsonProperty("$invalidContentActions")]
        public bool InvalidContentActions { get; set; }

        [JsonProperty("$invalidOutputs")]
        public bool InvalidOutputs { get; set; }

        [JsonProperty("$invalidCustomActions")]
        public bool InvalidCustomActions { get; set; }

        [JsonProperty("$invalid")]
        public bool Invalid { get; set; }

        [JsonProperty("$tags")]
        public List<Tag> Tags { get; set; }
    }

    public partial class Tag
    {
        [JsonProperty("background")]
        public string Background { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("canChangeBackground")]
        public bool CanChangeBackground { get; set; }
    }

    public partial class CustomAction
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("$title")]
        public string Title { get; set; }

        [JsonProperty("$invalid")]
        public bool Invalid { get; set; }

        [JsonProperty("settings")]
        public CustomActionSettings Settings { get; set; }

        [JsonProperty("$$hashKey")]
        public string HashKey { get; set; }
    }

    public partial class CustomActionSettings
    {
        [JsonProperty("variable")]
        public string Variable { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public string Action { get; set; }

        [JsonProperty("listName", NullValueHandling = NullValueHandling.Ignore)]
        public string ListName { get; set; }

        [JsonProperty("function", NullValueHandling = NullValueHandling.Ignore)]
        public string Function { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty("inputVariables", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> InputVariables { get; set; }

        [JsonProperty("outputVariable", NullValueHandling = NullValueHandling.Ignore)]
        public string OutputVariable { get; set; }

        [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }

        [JsonProperty("uri", NullValueHandling = NullValueHandling.Ignore)]
        public string Uri { get; set; }

        [JsonProperty("responseStatusVariable", NullValueHandling = NullValueHandling.Ignore)]
        public string ResponseStatusVariable { get; set; }

        [JsonProperty("responseBodyVariable", NullValueHandling = NullValueHandling.Ignore)]
        public string ResponseBodyVariable { get; set; }

        [JsonProperty("extras", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Extras { get; set; }

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }

        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        public string Gender { get; set; }
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }
    }

    public partial class ConditionOutput
    {
        [JsonProperty("stateId")]
        public string StateId { get; set; }

        [JsonProperty("$connId")]
        public string ConnId { get; set; }

        [JsonProperty("conditions")]
        public List<Condition> Conditions { get; set; }
    }

    public partial class Condition
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("comparison")]
        public string Comparison { get; set; }

        [JsonProperty("values")]
        public List<string> Values { get; set; }
        [JsonProperty("variable")]
        public string Variable { get; set; }
    }

    public partial class ContentAction
    {
        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public Action Action { get; set; }

        [JsonProperty("$invalid")]
        public bool Invalid { get; set; }

        [JsonProperty("$$hashKey")]
        public string HashKey { get; set; }

        [JsonProperty("input", NullValueHandling = NullValueHandling.Ignore)]
        public Input Input { get; set; }
    }

    public partial class Action
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("settings")]
        public ActionSettings Settings { get; set; }

        [JsonProperty("$cardContent")]
        public ActionCardContent CardContent { get; set; }
    }

    public partial class ActionCardContent
    {
        [JsonProperty("document")]
        public ActionSettings Document { get; set; }

        [JsonProperty("editable")]
        public bool Editable { get; set; }

        [JsonProperty("deletable")]
        public bool Deletable { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
    }

    public partial class ActionSettings
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("content")]
        public object Content { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty("rawContent")]
        public string RawContent { get; set; }
    }

    public partial struct ContentUnion
    {
        public ContentClass ContentClass;
        public string String;

        public bool IsNull => ContentClass == null && String == null;
    }

    public partial class ContentClass
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("interval")]
        public long Interval { get; set; }
    }

    public partial class Input
    {
        [JsonProperty("bypass")]
        public bool Bypass { get; set; }

        [JsonProperty("$cardContent")]
        public InputCardContent CardContent { get; set; }

        [JsonProperty("$invalid")]
        public bool Invalid { get; set; }
        [JsonProperty("variable", NullValueHandling = NullValueHandling.Ignore)]
        public string variable { get; set; }
    }

    public partial class InputCardContent
    {
        [JsonProperty("document")]
        public Document Document { get; set; }

        [JsonProperty("editable")]
        public bool Editable { get; set; }

        [JsonProperty("deletable")]
        public bool Deletable { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
    }

    public partial class Document
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public partial class DefaultOutput
    {
        [JsonProperty("stateId")]
        public string StateId { get; set; }

        [JsonProperty("$invalid")]
        public bool Invalid { get; set; }
    }

    public partial class Position
    {
        [JsonProperty("top")]
        public string Top { get; set; }

        [JsonProperty("left")]
        public string Left { get; set; }
    }

    public partial class QuickReply
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("options")]
        public List<Option> Options { get; set; }
    }

    public partial class Option
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("previewText")]
        public string PreviewText { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("type")]
        public object Type { get; set; }
    }

    public partial class DocumentCollection
    {
        [JsonProperty("itemType")]
        public string ItemType { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("options")]
        public List<Options> Options { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("editing")]
        public bool Editing { get; set; }
    }

    public partial class Header
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("aspectRatio")]
        public string AspectRatio { get; set; }
    }

    public partial class Options
    {
        [JsonProperty("label")]
        public Label Label { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public Label Value { get; set; }
    }

    public partial class Label
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }

    public partial class Weblink
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("uri")]
        public Uri Uri { get; set; }
    }

    public partial class BlipBuilderFlow
    {
        public static BlipBuilderFlow FromJson(string json) => JsonConvert.DeserializeObject<BlipBuilderFlow>(json);
    }

    public static class Serialize
    {
        public static string ToJson(this BlipBuilderFlow self) => JsonConvert.SerializeObject(self);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
