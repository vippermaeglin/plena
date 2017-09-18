using System;
//using M4.Utils;
using ModulusFE.Sockets;

namespace M4.DataServer.Interface.ProtocolStructs
{
    public class BarEventArgs : EventArgs
    {
        public BarData Bar { get; set; }
        public BarEventArgs(BarData bar)
        {
            Bar = bar;
        }
    }
    public class SymbolSnapshotPS : IParserStruct
    {
        public string ClientId { get; set; }
        public string MuxId { get; set; }
        public string RequestId { get; set; }
        public int BaseType { get; set; }
        public SymbolSnapshot Snapshot { get; set; }

        #region Overrides of IParserStruct

        public override int Id
        {
            get { return StructsIds.SymbolSnapshot_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.Write(ClientId ?? "");
            _bw.Write(MuxId ?? "");
            _bw.Write(RequestId ?? "");
            _bw.Write(BaseType);

            _bw.Write(Snapshot.Id);
            _bw.Write(Snapshot.Symbol ?? "");
            _bw.Write(Snapshot.Timestamp.Serialize());
            _bw.Write(Snapshot.Open);
            _bw.Write(Snapshot.High);
            _bw.Write(Snapshot.Low);
            _bw.Write(Snapshot.Close);
            _bw.Write(Snapshot.Quantity);
            _bw.Write(Snapshot.Buyer);
            _bw.Write(Snapshot.Seller);

            _bw.Write(Snapshot.BidPrice);
            _bw.Write(Snapshot.BidSize);
            _bw.Write(Snapshot.AskPrice);
            _bw.Write(Snapshot.AskSize);
            _bw.Write(Snapshot.VolumeFinancial);
            _bw.Write(Snapshot.VolumeTrades);
            _bw.Write(Snapshot.VolumeStocks);
            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);

            ClientId = _br.ReadString();
            MuxId = _br.ReadString();
            RequestId = _br.ReadString();
            BaseType = _br.ReadInt32();
            Snapshot = new SymbolSnapshot()
                               {
                                    Id = _br.ReadInt32(),
                                    Symbol = _br.ReadString(),
                                    Timestamp = DateTimeExtensions.DeSerialize(_br.ReadString()),
                                    Open = _br.ReadDouble(),
                                    High = _br.ReadDouble(),
                                    Low = _br.ReadDouble(),
                                    Close = _br.ReadDouble(),
                                    Quantity = _br.ReadInt32(),
                                    Buyer = _br.ReadInt32(),
                                    Seller = _br.ReadInt32(),
                                    BidPrice = _br.ReadDouble(),
                                    BidSize = _br.ReadInt32(),
                                    AskPrice = _br.ReadDouble(),
                                    AskSize = _br.ReadInt32(),
                                    VolumeFinancial = _br.ReadDouble(),
                                    VolumeTrades = _br.ReadInt32(),
                                    VolumeStocks = _br.ReadInt32(),
                               };

            StopR();
        }

        #endregion
    }
}
