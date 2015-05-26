namespace ETF.API.Service
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using ETF.API.Service.Interface;
    using ETF.PublicEntities;
    using ETF.Utilities;

    public class FileService : IFileService
    {
        public List<IndexEtf> GetIndexEtfList(byte[] content)
        {
            var indexEtfList = new List<IndexEtf>();

            Stream stream = new MemoryStream(content);

            var csvreader = new StreamReader(stream, System.Text.Encoding.UTF8, true);

            while (!csvreader.EndOfStream)
            {
                var line = csvreader.ReadLine();
                if (line == null)
                {
                    continue;
                }

                var values = line.Split(',');

                // If it is a header row, don't parse the row
                if (values[1] == "DATE")
                {
                    continue;
                }

                var indexName = values[0];
                var index = indexEtfList.SingleOrDefault(i => i.IndexName == indexName);

                if (index == null)
                {
                    index = new IndexEtf { IndexName = indexName };

                    indexEtfList.Add(index);
                }

                index.Stocks.Add(
                    new Stock
                        {
                            Date = values[1].ToDateTime(),
                            Id = values[2],
                            Name = values[3],
                            Price = values[4].ToDouble(),
                            ShareNumber = values[5].ToInt(0)
                        });
            }

            return indexEtfList;
        }
    }
}