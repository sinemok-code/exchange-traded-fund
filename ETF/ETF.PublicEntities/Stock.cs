namespace ETF.PublicEntities
{
    using System;

    public class Stock
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public double Price { get; set; }

        public int ShareNumber { get; set; }
    }
}
