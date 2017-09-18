// (c) Copyright Crainiate Software 2010

using System;
using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class Solids: Elements<Solid>
    {
        public Solids(Model model): base(model)
        {

        }

        //For deserialization only
        internal Solids(): base()
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
