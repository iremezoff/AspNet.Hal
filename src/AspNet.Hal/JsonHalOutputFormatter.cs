using System;
using System.Reflection;
using System.Threading.Tasks;
using AspNet.Hal.JsonConverters;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace AspNet.Hal
{
    public class JsonHalOutputFormatter : JsonOutputFormatter
    {
        readonly ResourceListConverter resourceListConverter = new ResourceListConverter();
        readonly ResourceConverter resourceConverter = new ResourceConverter();
        readonly LinksConverter linksConverter = new LinksConverter();
        readonly EmbeddedResourceConverter embeddedResourceConverter = new EmbeddedResourceConverter();
        readonly IHypermediaResolver hypermediaConfiguration;

        public JsonHalOutputFormatter(IHypermediaResolver hypermediaConfiguration)
        {
            if (hypermediaConfiguration == null) 
                throw new ArgumentNullException(nameof(hypermediaConfiguration));

            resourceConverter = new ResourceConverter(hypermediaConfiguration);
            Initialize();
        }

        public JsonHalOutputFormatter()
        {
            resourceConverter = new ResourceConverter();
            Initialize();
        }

        void Initialize()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
            SerializerSettings.Converters.Add(linksConverter);
            SerializerSettings.Converters.Add(resourceListConverter);
            SerializerSettings.Converters.Add(resourceConverter);
            SerializerSettings.Converters.Add(embeddedResourceConverter);
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        protected override JsonSerializer CreateJsonSerializer()
        {
            var baseJsonSerializer =  base.CreateJsonSerializer();


            return baseJsonSerializer;
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(Representation).IsAssignableFrom(type);
        }
    }
}