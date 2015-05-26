namespace ETF.PublicEntities
{
    using System.Collections.Generic;

    public class IndexEtf
    {
        public IndexEtf()
        {
            this.Stocks = new List<Stock>();
        }

        public string IndexName { get; set; }

        public List<Stock> Stocks { get; set; }
    }
}
