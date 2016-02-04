using System.Collections.Generic;
using AspNet.Hal.Interfaces;

namespace AspNet.Hal
{
    internal class EmbeddedResource
    {
        public EmbeddedResource()
        {
            Resources = new List<IResource>();
        }

        public bool IsSourceAnArray { get; set; }
        public IList<IResource> Resources { get; private set; }
    }
}