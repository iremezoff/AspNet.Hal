using System.IO;
using System.Net.Http;
using ApprovalTests;
using ApprovalTests.Reporters;
using AspNet.Hal.Test.Representations;
using Xunit;

namespace AspNet.Hal.Test
{
    public class HalResourceWithPeopleTest
    {
        readonly OrganisationWithPeopleRepresentation resource;

        public HalResourceWithPeopleTest()
        {
            resource = new OrganisationWithPeopleRepresentation(1, "Org Name");
        }

        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public void organisation_get_json_test()
        {
            // arrange
            var mediaFormatter = new JsonHalOutputFormatter {  };
            var content = new StringContent(string.Empty);
            var type = resource.GetType();

            // act
            using (var stream = new MemoryStream())
            {
                //mediaFormatter.WriteToStreamAsync(type, resource, stream, content, null).Wait();
                stream.Seek(0, SeekOrigin.Begin);
                var serialisedResult = new StreamReader(stream).ReadToEnd();

                // assert
                Approvals.Verify(serialisedResult);
            }
        }

        //[Fact]
        //[UseReporter(typeof(DiffReporter))]
        //public void organisation_get_xml_test()
        //{
        //    // arrange
        //    var mediaFormatter = new XmlHalMediaTypeFormatter();
        //    var content = new StringContent(string.Empty);
        //    var type = resource.GetType();



        //    // act
        //    using (var stream = new MemoryStream())
        //    {
        //        mediaFormatter.WriteToStreamAsync(type, resource, stream, content, null);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        var serialisedResult = new StreamReader(stream).ReadToEnd();

        //        // assert
        //        Approvals.Verify(serialisedResult);
        //    }
        //} 
    }
}