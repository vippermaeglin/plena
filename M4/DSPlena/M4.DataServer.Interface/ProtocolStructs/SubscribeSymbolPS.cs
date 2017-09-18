using ModulusFE.Sockets;

namespace M4.DataServer.Interface.ProtocolStructs
{
    public class SubscribeSymbolPS : IParserStruct
    {
        public string ClientId { get; set; }
        public string MuxId { get; set; }
        public BarData LastBar { get; set; }
        public bool Subscribe { get; set; }
        public string RequestId { get; set; }

        #region Overrides of IParserStruct

        public override int Id
        {
            get { return StructsIds.SubscribeSymbol_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.WriteEx(ClientId ?? "");
            _bw.WriteEx(MuxId ?? "");
            _bw.Write(LastBar.Symbol ?? "");
            _bw.Write(LastBar.BaseType);
            _bw.Write(LastBar.TradeDate.Serialize());
            _bw.Write(LastBar.OpenPrice);
            _bw.Write(LastBar.HighPrice);
            _bw.Write(LastBar.LowPrice);
            _bw.Write(LastBar.ClosePrice);
            _bw.Write(LastBar.VolumeF);
            _bw.Write(LastBar.VolumeS);
            _bw.Write(LastBar.VolumeT);
            _bw.Write(LastBar.AdjustD);
            _bw.Write(LastBar.AdjustS);
            _bw.Write(Subscribe);
            _bw.Write(RequestId ?? "");

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);

            ClientId = _br.ReadString();
            MuxId = _br.ReadString();
            LastBar = new BarData
            {
                Symbol = _br.ReadString(),
                BaseType = _br.ReadInt32(),
                TradeDate = DateTimeExtensions.DeSerialize(_br.ReadString()),
                OpenPrice = _br.ReadSingle(),
                HighPrice = _br.ReadSingle(),
                LowPrice = _br.ReadSingle(),
                ClosePrice = _br.ReadSingle(),
                VolumeF = _br.ReadDouble(),
                VolumeS = _br.ReadInt64(),
                VolumeT = _br.ReadInt64(),
                AdjustD = _br.ReadSingle(),
                AdjustS = _br.ReadSingle(),
            };
            Subscribe = _br.ReadBoolean();
            RequestId = _br.ReadString();

            StopR();
        }

        #endregion
    }
}
