using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ModulusFE.Sockets
{
  public class PolicyServer
  {
    private readonly byte[] _policy;
    private TcpListener _listener;

    public PolicyServer(string policyFile)
    {
      // Load the _policy file.
      FileStream policyStream = new FileStream(policyFile, FileMode.Open);
      _policy = new byte[policyStream.Length];
      policyStream.Read(_policy, 0, _policy.Length);
      policyStream.Close();
    }

    public void Start()
    {
      // Create the _listener.
      _listener = new TcpListener(IPAddress.Any, 943);
      _listener.Start();

      // Wait for a connection.
      _listener.BeginAcceptTcpClient(OnAcceptTcpClient, null);
    }

    public void OnAcceptTcpClient(IAsyncResult ar)
    {
      if (_isStopped) return;

      Console.WriteLine("Received policy request.");

      // Wait for the next connection.
      _listener.BeginAcceptTcpClient(OnAcceptTcpClient, null);

      // Handle this connection.
      try
      {
        TcpClient client = _listener.EndAcceptTcpClient(ar);

        PolicyConnection policyConnection = new PolicyConnection(client, _policy);
        policyConnection.HandleRequest();
      }
      catch (Exception err)
      {
        Console.WriteLine(err.Message);
      }
    }

    private bool _isStopped;
    public void Stop()
    {
      if (_isStopped) 
        return;

      _isStopped = true;

      try
      {
        _listener.Stop();
      }
      catch (Exception err)
      {
        Console.WriteLine(err.Message);
      }
    }
  }
}
