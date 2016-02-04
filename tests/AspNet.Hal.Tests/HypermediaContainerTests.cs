using System.IO;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using AspNet.Hal.Test.HypermediaAppenders;
using AspNet.Hal.Test.Representations;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;
using Moq;
using Xunit;

namespace AspNet.Hal.Test
{
    public class HypermediaContainerTests
    {
        readonly ProductRepresentation representation = new ProductRepresentation();

        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public void CanUseRegisterExtensionMethod()
        {
            var curie = new CuriesLink("aap", "http://www.helpt.com/{?rel}");

            var builder = Hypermedia.CreateBuilder();
            var selfLink = new Link<ProductRepresentation>("product", "http://www.product.com?id=1");
            var link2 = new Link("related", "http://www.related.com");
            var link3 = curie.CreateLink<CategoryRepresentation>("category", "http://www.category.com");

            builder.Register(selfLink, link2, link3);

            var config = builder.Build();

            // arrange
            var mediaFormatter = new JsonHalOutputFormatter(config) { };
            var type = representation.GetType();
            var httpContext = new Mock<HttpContext>();
            var httpRequest = new DefaultHttpContext().Request;
            httpContext.SetupGet(o => o.Request).Returns(httpRequest);

            // act
            using (var stream = new MemoryStream())
            {
                mediaFormatter.WriteResponseBodyAsync(new OutputFormatterWriteContext(httpContext.Object,
                    (s,e)=> new HttpResponseStreamWriter(s, e), type, representation));
                //WriteToStreamAsync(type, representation, stream, content, null);
                stream.Seek(0, SeekOrigin.Begin);
                var serialisedResult = new StreamReader(stream).ReadToEnd();

                // assert
                Approvals.Verify(serialisedResult);
            }
        }

        [Fact]
        public void CanRegisterAndResolveAHypermediaAppender()
        {
            var appender = new ProductRepresentationHypermediaAppender();
            var resource = new ProductRepresentation();
            var builder = Hypermedia.CreateBuilder();

            builder.RegisterAppender(appender);

            var config = builder.Build();
            var registered = config.ResolveAppender(resource);

            Assert.True(ReferenceEquals(appender, registered));

        }

        [Fact]
        public void ResolvesNullHypermediaAppenderIfNotRegistered()
        {
            var resource = new ProductRepresentation();
            var builder = Hypermedia.CreateBuilder();

            var config = builder.Build();
            var registered = config.ResolveAppender(resource);

            Assert.Null(registered);
        }

        [Fact]
        public void CanRegisterAndResolveASelfLinkForARepresentation()
        {
            const string href = "http://api.example.com/products/{id}";

            var link = new Link("example-namespace:product", href);
            var builder = Hypermedia.CreateBuilder();

            builder.RegisterSelf<ProductRepresentation>(link);

            var config = builder.Build();
            var registered = config.ResolveSelf(representation);

            Assert.Equal(Link.RelForSelf, registered.Rel);
            Assert.Equal(href, registered.Href);
        }

        [Fact]
        public void CanRegisterAndResolveALinkForASingleRepresentation()
        {
            var builder = Hypermedia.CreateBuilder();
            var link = new Link();

            builder.RegisterLinks<ProductRepresentation>(link);

            var config = builder.Build();
            var hypermedia = config.ResolveLinks(representation);

            Assert.Equal(1, hypermedia.Count());
            Assert.Equal(link, hypermedia.First(), Link.EqualityComparer);
        }

        [Fact]
        public void CanRegisterAndResolveMultipleLinksForASingleRepresentation()
        {
            var link1 = new Link("foo", "bar");
            var link2 = new Link("baz", "qux");
            var builder = Hypermedia.CreateBuilder();

            builder.RegisterLinks<ProductRepresentation>(link1, link2);

            var config = builder.Build();
            var hypermedia = config.ResolveLinks(representation);

            Assert.Equal(2, hypermedia.Count());
            Assert.Contains(link1, hypermedia, Link.EqualityComparer);
            Assert.Contains(link2, hypermedia, Link.EqualityComparer);
        }
    }
}
