using ModulusFE.Sockets;

namespace M4.DataServer.Interface.ProtocolStructs
{
  public class PingPS : IParserStruct
  {
    public Ping Ping { get; set; }

    #region Overrides of IParserStruct

    public override int Id
    {
      get { return StructsIds.Ping_Id; }
    }

    public override void WriteBytes(byte[] buffer, int offset)
    {
      StartW(buffer, offset);

      _bw.Write(Ping.RequestTimeStamp.Serialize());
      _bw.Write(Ping.AnswerTimeStamp.Serialize());
      _bw.Write(Ping.IntervalKeepAliveMillis);

      StopW();
    }

    public override void ReadBytes(byte[] buffer, int offset)
    {
      StartR(buffer, offset);

      Ping = new Ping
               {
                 RequestTimeStamp = DateTimeExtensions.DeSerialize(_br.ReadString()),
                 AnswerTimeStamp = DateTimeExtensions.DeSerialize(_br.ReadString()),
                 IntervalKeepAliveMillis = _br.ReadInt32()
               };

      StopR();
    }

    #endregion
  }
}
