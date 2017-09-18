// (c) Copyright Crainiate Software 2010

using System;
using System.Text;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class Elements : Elements<Element>
    {
        public Elements(): base(null)
        {
        }
        public override bool Remove(string key)
        {
            Element elem = base[key];

            if (base.Remove(key))
            {
                OnElementRemoved(elem);
                return true;
            }
            return false;
        }
    }
}
