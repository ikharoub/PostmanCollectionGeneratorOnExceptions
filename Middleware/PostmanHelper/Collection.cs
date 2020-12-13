using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PostmanCollectionGeneratorOnExceptions.Middleware.PostmanHelper
{

    public class Collection
    {
        [JsonIgnore]
        public string CollectionName { get; set; }
        public Collection()
        {
            CollectionName = $"{DateTime.Now:dd-MM-yyyy}.json";
            Info = new Info
            {
                Name = $"{DateTime.Now:dd-MM-yyyy}",
                PostmanId = Guid.NewGuid(),
                Schema = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
            };
        }

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("item")]
        public List<Item> Items { get; set; }

        [JsonProperty("protocolProfileBehavior")]
        public object ProtocolProfileBehavior { get; set; }
    }

    public class Info
    {
        [JsonProperty("_postman_id")]
        public Guid PostmanId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("schema")]
        public string Schema { get; set; }
    }

    public class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("protocolProfileBehavior")]
        public ItemProtocolProfileBehavior ProtocolProfileBehavior { get; set; }

        [JsonProperty("request")]
        public Request Request { get; set; }

        [JsonProperty("response")]
        public object[] Response { get; set; }
    }

    public class ItemProtocolProfileBehavior
    {
        [JsonProperty("disableBodyPruning")]
        public bool DisableBodyPruning { get; set; }
    }

    public class Request
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("header")]
        public List<Header> Header { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("url")]
        public Url Url { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Body
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("options")]
        public Options Options { get; set; }
    }

    public class Options
    {
        [JsonProperty("raw")]
        public Raw Raw { get; set; }
    }

    public class Raw
    {
        [JsonProperty("language")]
        public string Language { get; set; }
    }

    public class Header
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Url
    {
        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("host")]
        public string[] Host { get; set; }

        [JsonProperty("query")]
        public List<Query> Query { get; set; }

        [JsonProperty("path")]
        public string[] Path { get; set; }
    }

    public class Query
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }


}
