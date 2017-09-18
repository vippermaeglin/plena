using System;
using ModulusFE.Sockets;

namespace M4.DataServer.Interface.ProtocolStructs
{
    public class CommandRequestPS : IParserStruct
    {
        #region Overrides of IParserStruct

        public string RequestId { get; set; }
        public string ClientId { get; set; }
        public string MuxId { get; set; }
        public override int Id
        {
            get { return StructsIds.CommandRequest_Id; }
        }

        public CommandRequest Request { get; set; }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.Write(ClientId ?? "");
            _bw.Write(RequestId ?? "");
            _bw.Write(MuxId ?? "");
            _bw.Write(Request.CommandID);
            _bw.Write(Request.Date.Serialize());
            _bw.Write(Request.Parameters ?? "");

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);
            ClientId = _br.ReadString();
            RequestId = _br.ReadString();
            MuxId = _br.ReadString();
            Request = new CommandRequest
                        {
                            CommandID = _br.ReadInt32(),
                            Date = DateTimeExtensions.DeSerialize(_br.ReadString()),
                            Parameters = _br.ReadString()
                        };

            StopR();
        }

        #endregion
    }
}
