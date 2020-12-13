using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PostmanCollectionGeneratorOnExceptions.Middleware.PostmanHelper
{
    public static class CollectionWriter
    {
        // lock to handle concurrent exceptions  
        private static readonly object CollectionLock;

        static CollectionWriter()
        {
            CollectionLock = new object();
        }

        public static async Task WriteIntoCollection(HttpContext context, Exception e)
        {

            // reading request body.
            var stringfiedBody = "";

            context.Request.Body.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                stringfiedBody = await reader.ReadToEndAsync();
            }

            // creating post man request collection item.

            var collectionItem =
                new Item
                {
                    Name = e.GetType().Name,
                    Request = new Request
                    {
                        Url = new Url
                        {
                            Protocol = context.Request.IsHttps? "https" : "http",
                            Raw = context.Request.Host.Value,
                            Query = context.Request.Query.Select(x => new Query
                            {
                                Key = x.Key,
                                Value = x.Value
                            }).ToList(),
                            Host = new[] { context.Request.Host.Value },
                            Path = context.Request.Path.HasValue? new [] { context.Request.Path.Value } : new string[0]
                        },
                        Description = e.ToString(),
                        Method = context.Request.Method,
                        Header = context.Request.Headers.Select(x => new Header
                        {
                            Key = x.Key,
                            Value = x.Value,
                            Type = "text"
                        }).ToList(),
                        Body = new Body
                        {
                            Options = new Options
                            {
                                Raw = new Raw
                                {
                                    Language = "json"
                                }
                            },
                            Mode = "raw",
                            Raw = stringfiedBody,
                        }
                    },
                    ProtocolProfileBehavior = new ItemProtocolProfileBehavior
                    {
                        DisableBodyPruning = true
                    },
                    Response = new object[0]
                };



            // start locking
            lock (CollectionLock)
            {
                var collection = new Collection();
                var serializer = new JsonSerializer();

                // load collection if exist 
                if (File.Exists(collection.CollectionName))
                {
                    using var reader = File.OpenText(collection.CollectionName);
                    using var jsonReader = new JsonTextReader(reader);
                    collection = serializer.Deserialize<Collection>(jsonReader);
                }
                else
                {
                    collection.Items = new List<Item>();
                    collection.ProtocolProfileBehavior = new object();
                }

                // add request to collection
                collection.Items.Add(collectionItem);
                using var writer = File.CreateText(collection.CollectionName);


                // rewrite collection
                serializer.Serialize(writer, collection);
            }
        }
    }
}
