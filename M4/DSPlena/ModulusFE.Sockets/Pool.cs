using System.Collections.Generic;

namespace ModulusFE.Sockets
{
  public class Pool<T>
  {
    private readonly Stack<T> _pool;

    public Pool(int count)
    {
      _pool = new Stack<T>(count);
    }

    public void Push(T subscriber)
    {
      lock (this)
      {
        _pool.Push(subscriber);
      }
    }

    public T Pop()
    {
      lock (this)
      {
        return _pool.Pop();
      }
    }

    public int Count { get { return _pool.Count; } }
  }
}

