namespace ETF.API.Test.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ETF.API.Service;
    using ETF.API.Service.Interface;
    using ETF.PublicEntities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class EtfServiceTests
    {
        private Mock<IEtfContext> etfContext;

        private IEtfService etfService;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            this.etfContext = new Mock<IEtfContext>();
            this.SetupEtfValues();

            this.etfService = new EtfService(this.etfContext.Object);
        }

        [TestMethod]
        public void GetEtfWeightedIndices_StartDateAndEndDateNotSet_ResultGroupedByDate()
        {
            // Arrange
            DateTime? startDate = null;
            DateTime? endDate = null;

            // Act
            var result = this.etfService.GetEtfWeightedIndices(startDate, endDate);

            // Assert
            var etfWeightedIndices = result as EtfWeightedIndex[] ?? result.ToArray();
            Assert.AreEqual(etfWeightedIndices.Count(), 2);
        }

        [TestMethod]
        public void GetEtfWeightedIndices_StartDateAndEndDateNotSet_InitialIndexLevelIs100()
        {
            // Arrange
            DateTime? startDate = null;
            DateTime? endDate = null;

            // Act
            var result = this.etfService.GetEtfWeightedIndices(startDate, endDate);
            
            // Assert
            var etfWeightedIndices = result as EtfWeightedIndex[] ?? result.ToArray();
            var firstItem = etfWeightedIndices.OrderBy(p => p.Date).FirstOrDefault();
            Assert.AreEqual(firstItem.Value, 100);
        }

        [TestMethod]
        public void GetEtfWeightedIndices_StartDateAndEndDateNotSet_IndexLevelCalculated()
        {
            // Arrange
            DateTime? startDate = null;
            DateTime? endDate = null;

            // Act
            var result = this.etfService.GetEtfWeightedIndices(startDate, endDate);

            // Assert
            var etfWeightedIndices = result as EtfWeightedIndex[] ?? result.ToArray();
            var firstItem = etfWeightedIndices.OrderByDescending(p => p.Date).FirstOrDefault();
            Assert.AreEqual(firstItem.Value, 95);
        }

        private void SetupEtfValues()
        {
            var indexEtf = new IndexEtf
            {
                IndexName = "MyIndex",
                Stocks =
                    new List<Stock>
                                            {
                                                new Stock
                                                    {
                                                        Date = DateTime.Today,
                                                        Id = "StockAId",
                                                        Name = "StockA",
                                                        Price = 300,
                                                        ShareNumber = 100
                                                    },
                                                new Stock
                                                    {
                                                        Date = DateTime.Today,
                                                        Id = "StockBId",
                                                        Name = "StockB",
                                                        Price = 200,
                                                        ShareNumber = 150
                                                    },
                                                new Stock
                                                    {
                                                        Date = DateTime.Today,
                                                        Id = "StockCId",
                                                        Name = "StockC",
                                                        Price = 500,
                                                        ShareNumber = 50
                                                    },
                                                new Stock
                                                    {
                                                        Date = DateTime.Today.AddDays(1),
                                                        Id = "StockAId",
                                                        Name = "StockA",
                                                        Price = 400,
                                                        ShareNumber = 80
                                                    },
                                                new Stock
                                                    {
                                                        Date = DateTime.Today.AddDays(1),
                                                        Id = "StockBId",
                                                        Name = "StockB",
                                                        Price = 300,
                                                        ShareNumber = 100
                                                    },
                                                new Stock
                                                    {
                                                        Date = DateTime.Today.AddDays(1),
                                                        Id = "StockCId",
                                                        Name = "StockC",
                                                        Price = 750,
                                                        ShareNumber = 25
                                                    }
                                            }
            };

            this.etfContext.Setup(x => x.GetIndexEtfList()).Returns(new List<IndexEtf> { indexEtf });
        }
    }
}
