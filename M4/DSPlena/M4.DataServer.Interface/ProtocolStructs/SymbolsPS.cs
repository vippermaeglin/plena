using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModulusFE.Sockets;

namespace M4.DataServer.Interface.ProtocolStructs
{
    public class SymbolsPS : IParserStruct
    {
        public Symbol StockInfo;

        /// <summary>
        /// StockID represents the index
        /// </summary>
        public int StockId { get; set; }

        public string RequestId { get; set; }

        public string ClientId { get; set; }

        public string MuxId { get; set; }

        #region Overrides of IParserStruct

        public override int Id
        {
            get { return StructsIds.Symbols_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.Write(StockId);
            _bw.Write(RequestId ?? "");
            _bw.Write(ClientId ?? "");
            _bw.Write(MuxId ?? "");
            _bw.Write(StockInfo.Code ?? "");
            _bw.Write(StockInfo.Name ?? "");
            _bw.Write(StockInfo.Sector ?? "");
            _bw.Write(StockInfo.SubSector ?? "");
            _bw.Write(StockInfo.Segment ?? "");
            _bw.Write(StockInfo.Source ?? "");
            _bw.Write(StockInfo.Type ?? "");
            _bw.Write(StockInfo.Activity ?? "");
            _bw.Write(StockInfo.Site ?? "");
            _bw.Write(StockInfo.Status);
            _bw.Write(StockInfo.Priority);

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);

            StockId = _br.ReadInt32();
            RequestId = _br.ReadString();
            ClientId = _br.ReadString();
            MuxId = _br.ReadString();
            StockInfo = new Symbol
            {
                Code = _br.ReadString(),
                Name = _br.ReadString(),
                Sector = _br.ReadString(),
                SubSector = _br.ReadString(),
                Segment = _br.ReadString(),
                Source = _br.ReadString(),
                Type = _br.ReadString(),
                Activity = _br.ReadString(),
                Site = _br.ReadString(),
                Status = _br.ReadInt32(),
                Priority = _br.ReadInt32()
            };

            StopR();
        }

        #endregion
    }
    public class ListSymbolsPS : IParserStruct
    {
        public List<Symbol> StocksInfos;
        public int TotalBarsCount { get; set; }
        /// <summary>
        /// StockID represents the index
        /// </summary>
        public int StockId { get; set; }

        public string RequestId { get; set; }

        public string ClientId { get; set; }

        public string MuxId { get; set; }

        #region Overrides of IParserStruct

        public override int Id
        {
            get { return StructsIds.ListSymbols_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.Write(StockId);
            _bw.Write(RequestId ?? "");
            _bw.Write(ClientId ?? "");
            _bw.Write(MuxId ?? "");
            _bw.Write(TotalBarsCount);
            for (int i = 0; i < TotalBarsCount; i++)
            {
                _bw.Write(StocksInfos[i].Code ?? "");
                _bw.Write(StocksInfos[i].Name ?? "");
                _bw.Write(StocksInfos[i].Sector ?? "");
                _bw.Write(StocksInfos[i].SubSector ?? "");
                _bw.Write(StocksInfos[i].Segment ?? "");
                _bw.Write(StocksInfos[i].Source ?? "");
                _bw.Write(StocksInfos[i].Type ?? "");
                _bw.Write(StocksInfos[i].Activity ?? "");
                _bw.Write(StocksInfos[i].Site ?? "");
                _bw.Write(StocksInfos[i].Status);
                _bw.Write(StocksInfos[i].Priority);
            }

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);

            StockId = _br.ReadInt32();
            RequestId = _br.ReadString();
            ClientId = _br.ReadString();
            MuxId = _br.ReadString();
            TotalBarsCount = _br.ReadInt32();
            StocksInfos = new List<Symbol>();
            for (int i = 0; i < TotalBarsCount; i++)
            {
                StocksInfos.Add( new Symbol
                {
                    Code = _br.ReadString(),
                    Name = _br.ReadString(),
                    Sector = _br.ReadString(),
                    SubSector = _br.ReadString(),
                    Segment = _br.ReadString(),
                    Source = _br.ReadString(),
                    Type = _br.ReadString(),
                    Activity = _br.ReadString(),
                    Site = _br.ReadString(),
                    Status = _br.ReadInt32(),
                    Priority = _br.ReadInt32()
                });
            }

            StopR();
        }

        #endregion
    }

    public class SymbolRequestPS : IParserStruct
    {
        #region Overrides of IParserStruct

        /// <summary>
        /// Symbol ClientID, this way DataManager knows where to dispatch the message 
        /// </summary>
        public string RequestId { get; set; }
        public string ClientId { get; set; }
        public string MuxId { get; set; }
        public DateTime LastUpdate { get; set; }

        public override int Id
        {
            get { return StructsIds.SymbolsRequest_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            if (RequestId == null) ClientId = "NULL";
            if (ClientId == null) ClientId = "NULL";
            StartW(buffer, offset);
            _bw.Write(RequestId);
            _bw.Write(ClientId);
            _bw.Write(MuxId);
            _bw.Write(LastUpdate.Serialize());
            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);
            RequestId = _br.ReadString();
            ClientId = _br.ReadString();
            MuxId = _br.ReadString();
            LastUpdate = DateTimeExtensions.DeSerialize(_br.ReadString());
            StopR();
        }

        #endregion
    }

    public class SymbolGroupPS : IParserStruct
    {
        public SymbolGroup GroupInfo;

        /// <summary>
        /// StockID represents the index
        /// </summary>
        public int StockId { get; set; }

        public string RequestId { get; set; }

        public string ClientId { get; set; }

        public string MuxId { get; set; }

        #region Overrides of IParserStruct

        public override int Id
        {
            get { return StructsIds.SymbolGroup_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.Write(StockId);
            _bw.Write(RequestId ?? "");
            _bw.Write(ClientId ?? "");
            _bw.Write(MuxId ?? "");
            _bw.Write(GroupInfo.Name ?? "");
            _bw.Write(GroupInfo.Index);
            _bw.Write(GroupInfo.Type);
            _bw.Write(GroupInfo.Symbols);

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);

            StockId = _br.ReadInt32();
            RequestId = _br.ReadString();
            ClientId = _br.ReadString();
            MuxId = _br.ReadString();
            GroupInfo = new SymbolGroup
            {
                Name = _br.ReadString(),
                Index = _br.ReadInt32(),
                Type = _br.ReadInt32(),
                Symbols = _br.ReadString()
            };

            StopR();
        }

        #endregion
    }


}
