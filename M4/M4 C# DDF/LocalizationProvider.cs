using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls;

namespace M4
{
    public class LocalizationProvider : RadMessageLocalizationProvider
    {
        public string AbortButton = "Abortar";
        public string CancelButton = "Cancelar";
        public string IgnoreButton = "Ignorar";
        public string NoButton = "Não";
        public string OkButton = "OK";
        public string RetryButton = "Repetir";
        public string YesButton = "Sim";
        public LocalizationProvider()
        {

        }
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadMessageStringID.AbortButton: return AbortButton;
                case RadMessageStringID.CancelButton: return CancelButton;
                case RadMessageStringID.IgnoreButton: return IgnoreButton;
                case RadMessageStringID.NoButton: return NoButton;
                case RadMessageStringID.OKButton: return OkButton;
                case RadMessageStringID.RetryButton: return RetryButton;
                case RadMessageStringID.YesButton: return YesButton;
                default:
                    return base.GetLocalizedString(id);
            }
        }
    }
}
