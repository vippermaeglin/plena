using ModulusFE.Sockets;
using System.Collections.Generic;

namespace M4.DataServer.Interface.ProtocolStructs
{
    public class BarDataPS : IParserStruct
    {
        public BarData Data;

        /// <summary>
        /// Will have the total number of bars returned by DataServer
        /// </summary>
        public int TotalBarsCount { get; set; }

        /// <summary>
        /// Current bar index 
        /// </summary>
        public int BarIndex { get; set; }

        /// <summary>
        /// Historical RequestID, this way DataManager knows where to dispatch the message 
        /// </summary>
        public string RequestId { get; set; }
        public string ClientId { get; set; }
        public string MuxId { get; set; }

        public BarDataPS()
        {
        }

        public BarDataPS(BarData bar)
        {
            Data = bar;
        }

        #region Overrides of IParserStruct

        public override int Id
        {
            get { return StructsIds.BarData_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.Write(RequestId ?? "");
            _bw.Write(ClientId ?? "");
            _bw.Write(MuxId ?? "");
            _bw.Write(Data.Symbol ?? "");
            _bw.Write(Data.BaseType);
            _bw.Write(Data.TradeDate.Serialize());
            _bw.Write(Data.OpenPrice);
            _bw.Write(Data.HighPrice);
            _bw.Write(Data.LowPrice);
            _bw.Write(Data.ClosePrice);
            _bw.Write(Data.VolumeF);
            _bw.Write(Data.VolumeS);
            _bw.Write(Data.VolumeT);
            _bw.Write(Data.AdjustD);
            _bw.Write(Data.AdjustS);
            _bw.Write(BarIndex);
            _bw.Write(TotalBarsCount);

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);

            RequestId = _br.ReadString();
            ClientId = _br.ReadString();
            MuxId = _br.ReadString();
            Data = new BarData
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
                         AdjustS = _br.ReadSingle()
                     };
            Data.TradeDateTicks = Data.TradeDate.Ticks;
            BarIndex = _br.ReadInt32();
            TotalBarsCount = _br.ReadInt32();

            StopR();
        }

        #endregion
    }

    public class BarsDatasPS : IParserStruct
    {
        public List<BarData> Data;

        /// <summary>
        /// Will have the total number of bars returned by DataServer
        /// </summary>
        public int TotalBarsCount { get; set; }

        /// <summary>
        /// Current bar index 
        /// </summary>
        public int BarIndex { get; set; }

        /// <summary>
        /// Historical RequestID, this way DataManager knows where to dispatch the message 
        /// </summary>
        public string RequestId { get; set; }
        public string ClientId { get; set; }
        public string MuxId { get; set; }

        public BarsDatasPS()
        {
        }

        public BarsDatasPS(List<BarData> bar)
        {
            Data = bar;
        }

        #region Overrides of IParserStruct

        public override int Id
        {
            get { return StructsIds.BarsDatas_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.Write(RequestId ?? "");
            _bw.Write(ClientId ?? "");
            _bw.Write(MuxId ?? "");
            _bw.Write(TotalBarsCount);
            for (int i = 0; i < TotalBarsCount; i++)
            {
                _bw.Write(Data[i].Symbol ?? "");
                _bw.Write(Data[i].BaseType);
                _bw.Write(Data[i].TradeDate.Serialize());
                _bw.Write(Data[i].OpenPrice);
                _bw.Write(Data[i].HighPrice);
                _bw.Write(Data[i].LowPrice);
                _bw.Write(Data[i].ClosePrice);
                _bw.Write(Data[i].VolumeF);
                _bw.Write(Data[i].VolumeS);
                _bw.Write(Data[i].VolumeT);
                _bw.Write(Data[i].AdjustD);
                _bw.Write(Data[i].AdjustS);
            }
            _bw.Write(BarIndex);

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);

            RequestId = _br.ReadString();
            ClientId = _br.ReadString();
            MuxId = _br.ReadString();
            TotalBarsCount = _br.ReadInt32();
            Data = new List<BarData>();
            for (int i = 0; i < TotalBarsCount; i++)
            {
                Data.Add(new BarData
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
                });
                Data[i].TradeDateTicks = Data[i].TradeDate.Ticks;
            }
            BarIndex = _br.ReadInt32();

            StopR();
        }

        #endregion
    }
}
