using System;

namespace M4.DataServer.Interface
{
    public partial class CommandRequest
    {
        public int CommandID { get; set; }

        public DateTime Date { get; set; }

        public string Parameters { get; set; }
    }

    public enum ServerCommand
    {
        SyncUpdate,
        RemoveAllBars,
        RemoveBarsForSymbol,
        ReSyncronize
    }
}
