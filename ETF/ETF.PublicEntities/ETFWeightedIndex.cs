namespace ETF.PublicEntities
{
    using System;

    public class EtfWeightedIndex
    {
        public string IndexName { get; set; }

        public DateTime Date { get; set; }

        public double Value { get; set; }
    }
}
