$(function () {

    // Declare a proxy to reference the hub.
    var market = $.connection.marketHub;

    // Get a time stamp
    var timestamp = ((new Date()).getTime() / 1000) | 0;

    var previousPrice = 0;

    // Declare area chart
    var areaChart = $('#areaChart').epoch({
        type: 'time.area',
        data: [{ values: [] }],
        axes: ['left', 'bottom', 'right']
    });

    // Declare line chart
    var lineChart = $('#lineChart').epoch({
        type: 'time.line',
        data: [{ values: [] }],
        axes: ['left', 'bottom', 'right']
    });

    // Declare gauge chart
    var gaugeChart = $('#gaugeChart').epoch({
        type: 'time.gauge',
        value: previousPrice
    });

    // This invoked from the server side
    market.client.broadcastPrice = function (exchange, curr1, price) {

        // Update the area chart
        areaChart.push([{ time: timestamp, y: price }]);

        // Update the line chart
        lineChart.push([{ time: timestamp, y: price }]);

        // If the previous price has not been initialized, make the previous price the current price.
        previousPrice = previousPrice == 0 ? price : previousPrice;

        // Calculate performance
        var performance = price / previousPrice;

        // Update the gauge chart
        gaugeChart.update(performance);

        // Increase time stamp
        timestamp++;

        console.log(price);

        previousPrice = price;
    };

    // Log errors
    market.client.onError = function (error) {
        console.log(error);
    };

    // Start the connection to the market hub
    $.connection.hub.start();

});