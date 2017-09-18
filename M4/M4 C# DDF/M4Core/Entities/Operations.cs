using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M4Core.Enums;

namespace M4Core.Entities
{
    public class Operations
    {
        public string OID;
        public TypeOperations OType;
        public object[] OParams;

        public Operations(string id, TypeOperations type, object[] oparams)
        {
            OID = id;
            OType = type;
            OParams = oparams;
        }
    }
}
