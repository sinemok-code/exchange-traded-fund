namespace ETF.PublicEntities
{
    using System;

    /// <summary>
    /// Stocks returns for specific date
    /// </summary>
    public class StockReturn
    {
        public DateTime Date { get; set; }

        public double Value { get; set; }
    }
}
