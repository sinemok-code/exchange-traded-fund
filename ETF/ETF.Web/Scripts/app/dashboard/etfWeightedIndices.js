$(function() {

    initializeControls();
    initializeCharts();
    
    $("#updateDates").click(function () {
        initializeCharts();
    });

    function initializeControls() {
        $("#startDate").datepicker();
        $("#endDate").datepicker();
    }

    function initializeCharts() {
        initializeEtfWeightedIndices();
        initializeStockWeightedIndices();
        initializeTopStocksChart();
    }

    function initializeEtfWeightedIndices() {
        $.ajax({
            url: "http://localhost:64877/ETF/EtfWeightedIndex",
            data: { startDate: getStartDate(), endDate: getEndDate() }
        })
        .done(function (data) {
            populateEtfWeightedIndicesChart(data);
        });
    }

    var indexData = function (indexName) {
        this.name = indexName;
        this.data = [];

        this.equals = function (name) { return name === this.name; };
    };

    function populateEtfWeightedIndicesChart(data) {
        var dates = [];
        var chartSeries = [];

        data.forEach(function (etfWeightedIndex) {
            dates.push(new Date(etfWeightedIndex.Date).toDateString());
        });

        data.forEach(function (etfWeightedIndex) {
            var indexElement = null;
            chartSeries.forEach(function (indexDatum) {
                if (indexDatum.equals(etfWeightedIndex.IndexName)) {
                    indexElement = indexDatum;
                }
            });

            if (indexElement == null) {
                indexElement = new indexData(etfWeightedIndex.IndexName);

                chartSeries.push(indexElement);
            }

            indexElement.data.push(etfWeightedIndex.Value);
        });

        $('#etfWeightedIndicesContainer').highcharts({
            title: {
                text: 'ETF Index Level',
                x: -20 //center
            },
            xAxis: {
                categories: dates
            },
            yAxis: {
                title: {
                    text: 'ETF Index'
                },
                plotLines: [
                    {
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }
                ]
            },
            tooltip: {
                valueSuffix: ''
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: chartSeries
        });
    }

    function initializeStockWeightedIndices() {
        $.ajax({
            url: "http://localhost:64877/Stock/StockWeightedIndex",
            data: { startDate: getStartDate(), endDate: getEndDate() }
        })
        .done(function (data) {
            populateStockWeightedIndices(data);
        });
    }

    function populateStockWeightedIndices(stockWeightedIndices) {
        $('#stockWeightedIndicesContainer').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="stockWeightedIndicesTable"></table>');

        $('#stockWeightedIndicesTable').dataTable({
            "data": stockWeightedIndices,
            "columns": [
                {
                    "data": "Stock.Id", 
                    "title": "Stock ID"
                },
                {
                    "data": "Stock.Name", 
                    "title": "Stock Name"
                },
                {
                    "data": "Stock.Date", 
                    "title": "Date",
                    "render": function ( data, type, full, meta ) {
                        return new Date(data).toDateString();
                    }
                },
                {
                    "data": "Stock.Price", 
                    "title": "Stock Price"
                },
                {
                    "data": "Stock.ShareNumber", 
                    "title": "# of Share"
                },
                {
                    "data": "Value", 
                    "title": "Weighted Index"
                }
            ]
        });
    }

    function initializeTopStocksChart() {
        $.ajax({
            url: "http://localhost:64877/Stock/TopStockIndices",
            data: { endDate: getEndDate() }
        })
        .done(function (data) {
            populateTopStocksChart(data);
        });
    }

    var pieChartData = function(name, value) {
        this.name = name + " " + value;
        this.y = value;
    }

    function populateTopStocksChart(data) {
        var chartSeriesData = [];

        data.forEach(function (stock) {
            chartSeriesData.push(new pieChartData(stock.Stock.Name, stock.Value));
        });

        $('#topStocksContainer').highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false
            },
            title: {
                text: 'Top five weighted stocks for the latest date '
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Share of weight',
                data: chartSeriesData
            }]
        });
    }

    function getStartDate() {
        return $("#startDate").val();
    }

    function getEndDate() {
        return $("#endDate").val();
    }
});