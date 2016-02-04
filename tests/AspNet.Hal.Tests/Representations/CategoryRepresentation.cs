using Newtonsoft.Json;

namespace AspNet.Hal.Test.Representations
{
    public class CategoryRepresentation : Representation
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Title { get; set; }
    }
}