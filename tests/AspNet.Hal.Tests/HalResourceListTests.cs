using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using ApprovalTests;
using ApprovalTests.Reporters;
using AspNet.Hal.Test.Representations;
using Xunit;

namespace AspNet.Hal.Test
{
    public class HalResourceListTests
    {
        readonly OrganisationListRepresentation representation;

        readonly OrganisationListRepresentation oneitemrepresentation;

        public HalResourceListTests()
        {
            representation = new OrganisationListRepresentation(
                new List<OrganisationRepresentation>
                       {
                           new OrganisationRepresentation(1, "Org1"),
                           new OrganisationRepresentation(2, "Org2")
                       });

            oneitemrepresentation = new OrganisationListRepresentation(
                new List<OrganisationRepresentation>
                       {
                           new OrganisationRepresentation(1, "Org1")
                       });
        }

        //[Fact]
        //[UseReporter(typeof(DiffReporter))]
        //public void organisation_list_get_xml_test()
        //{
        //    // arrange
        //    var mediaFormatter = new XmlHalMediaTypeFormatter();
        //    var content = new StringContent(string.Empty);
        //    var type = representation.GetType();

        //    // act
        //    using (var stream = new MemoryStream())
        //    {
        //        mediaFormatter.WriteToStream(type, representation, stream, content);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        var serialisedResult = new StreamReader(stream).ReadToEnd();

        //        // assert
        //        Approvals.Verify(serialisedResult);
        //    }
        //}

        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public void organisation_list_get_json_test()
        {
            // arrange
            var mediaFormatter = new JsonHalOutputFormatter {  };
            var content = new StringContent(string.Empty);
            var type = representation.GetType();

            // act
            using (var stream = new MemoryStream())
            {
                //mediaFormatter.WriteToStreamAsync(type, representation, stream, content, null).Wait();
                stream.Seek(0, SeekOrigin.Begin);
                var serialisedResult = new StreamReader(stream).ReadToEnd();

                // assert
                Approvals.Verify(serialisedResult);
            }


        }

        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public void one_item_organisation_list_get_json_test()
        {
            // arrange
            var mediaFormatter = new JsonHalOutputFormatter {  };
            var content = new StringContent(string.Empty);
            var type = oneitemrepresentation.GetType();

            // act
            using (var stream = new MemoryStream())
            {
                //mediaFormatter.WriteToStreamAsync(type, oneitemrepresentation, stream, content, null).Wait();
                stream.Seek(0, SeekOrigin.Begin);
                var serialisedResult = new StreamReader(stream).ReadToEnd();

                // assert
                Approvals.Verify(serialisedResult);
            }
        }
    }
}