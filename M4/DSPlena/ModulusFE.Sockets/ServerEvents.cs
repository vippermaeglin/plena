using System;
using System.Net;

namespace ModulusFE.Sockets
{
  public delegate void StartingHandler(object sender);

  public delegate void StartedHandler(object sender);

  public delegate void IncommingConnectionHandler(object sender, IPEndPoint clientEndPoint);

  public delegate void ExceptionHandler(object sender, Exception exception, string methodName);

  public delegate void ClientDisconnectedHandler(object sender, string clientId);

  public delegate void MessageHandler(object sender, string message);

  public delegate void StopingHandler(object sender);

  public delegate void StopedHandler(object sender);

  public delegate void AuthenticateClientHandler(object sender, string username, string password, ref bool authenticated);

  public interface IServerEvents
  {
    event StartingHandler Starting;
    event StartingHandler Started;
    event IncommingConnectionHandler IncommingConnection;
    event ExceptionHandler Exception;
    event ClientDisconnectedHandler ClientDisconnected;
    event MessageHandler Message;
    event StopingHandler Stoping;
    event StopedHandler Stoped;
    event AuthenticateClientHandler AuthenticateClient;
  }
}
