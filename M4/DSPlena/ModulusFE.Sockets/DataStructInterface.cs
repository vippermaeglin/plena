using System.IO;

namespace ModulusFE.Sockets
{
  public abstract class IParserStruct
  {
    protected BinaryWriter _bw;
    protected BinaryReader _br;
        
    private MemoryStream _ms;
    private int _initialStreamPosition;

    protected void StartW(byte[] buffer, int offset)
    {
      _ms = new MemoryStream(buffer);
      _ms.Seek(offset, SeekOrigin.Begin);
      _initialStreamPosition = offset;
      _bw = new BinaryWriter(_ms);

      _bw.Write(Id);
      _bw.Write(Length); //zero initially
    }

    protected void StopW()
    {
      Length = (int)(_ms.Position - _initialStreamPosition); //Length can't be used cause it returns always 1024, the size of byte buffer assigned to stream

      _bw.Seek(_initialStreamPosition + sizeof (int), SeekOrigin.Begin);
      _bw.Write(Length); //write structure length after its id

      _bw.Close();
      _ms.Dispose();
    }

    protected void StartR(byte[] buffer, int offset)
    {
      _ms = new MemoryStream(buffer);
      _ms.Seek(offset, SeekOrigin.Begin);
      _br = new BinaryReader(_ms);      
    }

    protected void StopR()
    {
      _br.Close();
      _ms.Dispose();
    }

    public int Length { get; private set; }

    public abstract int Id { get; }
    public abstract void WriteBytes(byte[] buffer, int offset);
    public abstract void ReadBytes(byte[] buffer, int offset);
  }

  public static class StrUtils
  {
    public static int StrLen(string str)
    {
      return string.IsNullOrEmpty(str) ? 0 : str.Length;
    }
  }

  public static class Extensions
  {
    public static void WriteEx(this BinaryWriter self, string text)
    {
      self.Write(string.IsNullOrEmpty(text) ? "" : text);  
    }
  }
}
