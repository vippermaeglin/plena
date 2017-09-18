using ModulusFE.Sockets;

namespace M4.DataServer.Interface.ProtocolStructs
{
    public class TickDataPS : IParserStruct
    {
        public TickData TickData { get; set; }

        #region Overrides of IParserStruct

        public override int Id
        {
            get { return StructsIds.TickData_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.Write(TickData.Id);
            _bw.Write(TickData.Symbol ?? "");
            _bw.Write(TickData.TradeDate.Serialize());
            _bw.Write(TickData.Price);
            _bw.Write(TickData.Quantity);
            _bw.Write(TickData.Buyer);
            _bw.Write(TickData.Seller);

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);

            TickData = new TickData
                               {
                                   Id = _br.ReadInt32(),
                                   Symbol = _br.ReadString(),
                                   TradeDate = DateTimeExtensions.DeSerialize(_br.ReadString()),
                                   Price = _br.ReadSingle(),
                                   Quantity = _br.ReadInt32(),
                                   Buyer = _br.ReadInt32(),
                                   Seller = _br.ReadInt32()
                               };

            StopR();
        }

        #endregion
    }
}
