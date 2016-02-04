using System.Collections.Generic;
using AspNet.Hal.Test.Representations;

namespace AspNet.Hal.Test.HypermediaAppenders
{
    public class CategoryRepresentationHypermediaAppender : IHypermediaAppender<CategoryRepresentation>
    {
        public void Append(CategoryRepresentation resource, IEnumerable<Link> configured)
        {
            foreach (var link in configured)
            {
                switch (link.Rel)
                {
                    case Link.RelForSelf:
                        resource.Links.Add(link.CreateLink(new { id = resource.Id }));
                        break;
                    default:
                        resource.Links.Add(link); // append untouched ...
                        break;
                }
            }
        }
    }
}