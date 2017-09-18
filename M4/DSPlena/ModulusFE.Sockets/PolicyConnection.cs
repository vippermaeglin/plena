using System.IO;
using System.Net.Sockets;

namespace ModulusFE.Sockets
{
  public class PolicyConnection
  {
    private readonly TcpClient _client;
    private readonly byte[] _policy;

    public PolicyConnection(TcpClient client, byte[] policy)
    {
      _client = client;
      _policy = policy;
    }

    // The request that the _client sends.
    private const string PolicyRequestString = "<policy-file-request/>";

    public void HandleRequest()
    {
      Stream s = _client.GetStream();

      // Read the _policy request string.
      byte[] buffer = new byte[PolicyRequestString.Length];
      // Only wait 5 seconds.
      _client.ReceiveTimeout = 5000;
      s.Read(buffer, 0, buffer.Length);

      // Send the _policy.
      s.Write(_policy, 0, _policy.Length);

      // Close the connection.
      _client.Close();
    }
  }
}
