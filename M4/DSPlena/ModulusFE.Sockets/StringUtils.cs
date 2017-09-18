using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulusFE.Sockets
{
  public static class StringUtils
  {
    public static string Merge<T>(this IEnumerable<T> self, string delimiter)
    {
      var sb = new StringBuilder();
      foreach (T t in self)
      {
        if (sb.Length > 0) sb.Append(delimiter);
        sb.Append(t.ToString());
      }
      return sb.ToString();
    }

    public static string Merge<T, TResult>(this IEnumerable<T> self, string delimiter, Func<T, TResult> convertor)
    {
      var sb = new StringBuilder();
      foreach (T t in self)
      {
        if (sb.Length > 0) sb.Append(delimiter);
        sb.Append(convertor(t).ToString());
      }
      return sb.ToString();
    }

    public static string Merge<T>(this IEnumerable<T> self, string delimiter, string valueEncloser)
    {
      var sb = new StringBuilder();
      foreach (T t in self)
      {
        if (sb.Length > 0) sb.Append(delimiter);
        sb.Append(valueEncloser);
        sb.Append(t.ToString());
        sb.Append(valueEncloser);
      }
      return sb.ToString();
    }

    public static string Merge<T, TResult>(this IEnumerable<T> self, string delimiter, string valueEncloser, Func<T, TResult> convertor)
    {
      var sb = new StringBuilder();
      foreach (T t in self)
      {
        if (sb.Length > 0) sb.Append(delimiter);
        sb.Append(valueEncloser);
        sb.Append(convertor(t).ToString());
        sb.Append(valueEncloser);
      }
      return sb.ToString();
    }
  }
}
