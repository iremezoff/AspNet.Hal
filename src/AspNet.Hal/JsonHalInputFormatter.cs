using System;
using System.Reflection;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace AspNet.Hal
{
    public class JsonHalInputFormatter : JsonInputFormatter
    {
        readonly IHypermediaResolver hypermediaConfiguration;

        public JsonHalInputFormatter(IHypermediaResolver hypermediaConfiguration)
        {
            if (hypermediaConfiguration == null) 
                throw new ArgumentNullException(nameof(hypermediaConfiguration));

            Initialize();
        }

        public JsonHalInputFormatter()
        {
            Initialize();
        }

        void Initialize()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return typeof(Representation).IsAssignableFrom(context.ModelType);
        }
    }
}