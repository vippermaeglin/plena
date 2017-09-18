using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M4Core.Enums
{
    public enum TypeOperations
    {
        // Load symbol's list from database:
        InitializeSymbolDatabase,
        // Create new chart and new document:
        CreateNewCtlPainelChart,
        // Load chart on existent document:
        LoadCtlPainelChart,
        LoadSelectView
    }
}
