using System.Collections.Generic;
using AspNet.Hal.Interfaces;

namespace AspNet.Hal
{
    public interface IHypermediaAppender<T> where T:class, IResource
    {
        void Append(T resource, IEnumerable<Link> configured);
    }
}