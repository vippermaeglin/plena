using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ModulusFE.Sockets
{
  /// <summary>
  /// This class creates a single large buffer which can be divided up and assigned to SocketAsyncEventArgs objects for use
  /// with each socket I/O operation.  This enables bufffers to be easily reused and gaurds against fragmenting heap memory.
  /// 
  /// The operations exposed on the BufferManager class are not thread safe.
  /// </summary>
  internal class BufferManager
  {
    private readonly int m_numBytes;                 // the total number of bytes controlled by the buffer pool
    private byte[] m_buffer;                // the underlying byte array maintained by the Buffer Manager
    readonly Stack<int> m_freeIndexPool;     // 
    private int m_currentIndex;
    private readonly int m_bufferSize;

    public BufferManager(int totalBytes, int bufferSize)
    {
      m_numBytes = totalBytes;
      m_currentIndex = 0;
      m_bufferSize = bufferSize;
      m_freeIndexPool = new Stack<int>();
    }

    /// <summary>
    /// Allocates buffer space used by the buffer pool
    /// </summary>
    public void InitBuffer()
    {
      // create one big large buffer and divide that out to each SocketAsyncEventArg object
      m_buffer = new byte[m_numBytes];
    }

    /// <summary>
    /// Assigns a buffer from the buffer pool to the specified SocketAsyncEventArgs object
    /// </summary>
    /// <returns>true if the buffer was successfully set, else false</returns>
    public bool SetBuffer(SocketAsyncEventArgs args)
    {
      if (m_freeIndexPool.Count > 0)
      {
        args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
      }
      else
      {
        if ((m_numBytes - m_bufferSize) < m_currentIndex)
        {
          return false;
        }
        args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
        m_currentIndex += m_bufferSize;
      }
      return true;
    }

    public byte[] GetBuffer(out int offset)
    {
      offset = -1;
      if (m_freeIndexPool.Count > 0)
      {
        offset = m_freeIndexPool.Pop();
        return m_buffer;
      }

      if ((m_numBytes - m_bufferSize) < m_currentIndex)
      {
        return null;
      }
      offset = m_currentIndex;
      m_currentIndex += m_bufferSize;
      return m_buffer;
    }
    /// <summary>
    /// Removes the buffer from a SocketAsyncEventArg object.  This frees the buffer back to the 
    /// buffer pool
    /// </summary>
    public void FreeBuffer(SocketAsyncEventArgs args)
    {
      m_freeIndexPool.Push(args.Offset);
      args.SetBuffer(null, 0, 0);
    }
  }
}

