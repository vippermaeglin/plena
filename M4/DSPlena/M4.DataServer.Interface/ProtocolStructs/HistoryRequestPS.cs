using System;
using ModulusFE.Sockets;
using System.Collections.Generic;

namespace M4.DataServer.Interface.ProtocolStructs
{
    public class HistoryRequestPS : IParserStruct
    {
        #region Overrides of IParserStruct

        public string ClientId { get; set; }
        public string MuxId { get; set; }

        public int RequestCount { get; set; }
        public override int Id
        {
            get { return StructsIds.HistoricalRequest_Id; }
        }

        public List<HistoryRequest> Request { get; set; }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.Write(ClientId ?? "");
            _bw.Write(MuxId ?? "");
            _bw.Write(RequestCount);
            for (int i = 0; i < RequestCount; i++)
            {
                _bw.Write(Request[i].RequestId ?? "");
                _bw.Write(Request[i].Symbol ?? "");
                _bw.Write((short)Request[i].Periodicity);
                _bw.Write(Request[i].LastRecordTime.Serialize());
                _bw.Write(Request[i].LastRecordValue);
                _bw.Write(Request[i].BarSize);
                _bw.Write(Request[i].BarCount);
            }

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);
            ClientId = _br.ReadString();
            MuxId = _br.ReadString();
            RequestCount = _br.ReadInt32();
            Request = new List<HistoryRequest>();
            for (int i = 0; i < RequestCount; i++)
            {
                Request.Add(new HistoryRequest
                            {
                                RequestId = _br.ReadString(),
                                Symbol = _br.ReadString(),
                                Periodicity = (Periodicity)_br.ReadInt16(),
                                LastRecordTime = DateTimeExtensions.DeSerialize(_br.ReadString()),
                                LastRecordValue = _br.ReadSingle(),
                                BarSize = _br.ReadInt32(),
                                BarCount = _br.ReadInt32()
                            });
            }

            StopR();
        }

        #endregion
    }
}
