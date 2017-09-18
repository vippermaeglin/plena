using System.Globalization;

namespace M4
{
  public static class Format
  {
    public static CultureInfo ciUS = new CultureInfo("en-us");

    public static string ToUsCurrency(double value)
    {
      return value.ToString("c", ciUS);
    }

    public static double FromUsCurrency(object value)
    {
      return double.Parse(value.ToString(), NumberStyles.Currency, ciUS);
    }

    public static string ToLocalInteger(int value)
    {
      return value.ToString("N0");
    }

    public static int FromLocalInteger(object value)
    {
      return value == null ? 0 : int.Parse(value.ToString(), NumberStyles.Number);
    }
  }
}
