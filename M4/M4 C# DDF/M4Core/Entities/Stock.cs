namespace M4Core.Entities
{
    //For minute bars, hour bars, etc.
    public enum Periodicity
    {
        Secondly = 1,
        Minutely = 2,
        Hourly = 3,
        Daily = 4,
        Weekly = 5,
        Month = 6,
        Year = 7
    }

    public class Stock
    {
        #region Propriedades

        public string Code { get; set; }

        public string Name { get; set; }

        public string Sector { get; set; }

        public string Source { get; set; }

        public string CodeName { get; set; }

        #endregion
    }
}