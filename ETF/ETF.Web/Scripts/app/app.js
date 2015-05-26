(function() {

    var app = angular.module('etfApp',
    ['ngRoute']);

    app.config([
        '$routeProvider', function($routeProvider) {
            var viewBase = '/app/views/';

            $routeProvider
                .when('/dashboard', {
                    controller: 'DashboardController',
                    templateUrl: viewBase + 'customers/customers.html'
                })
                .otherwise({ redirectTo: '/dashboard' });
        }
    ]);
}());