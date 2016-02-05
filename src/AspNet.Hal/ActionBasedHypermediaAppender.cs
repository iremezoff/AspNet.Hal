using System;
using System.Collections.Generic;
using AspNet.Hal.Interfaces;

namespace AspNet.Hal
{
    public sealed class ActionBasedHypermediaAppender<T> : IHypermediaAppender<T> where T : class, IResource
    {
        readonly Action<T, IEnumerable<Link>> appendAction;

        public ActionBasedHypermediaAppender(Action<T, IEnumerable<Link>> appendAction)
        {
            if (appendAction == null) 
                throw new ArgumentNullException("appendAction");

            this.appendAction = appendAction;
        }

        public void Append(T resource, IEnumerable<Link> configured)
        {
            appendAction(resource, configured);
        }
    }
}