/// <reference path="scripts\jquery-2.1.1.js" />
/// <reference path="scripts\jquery.signalr-2.1.2.js" />
/// <reference path="scripts\angular.js" />

var app = angular.module('app', []);

app.controller('myController', function ($scope) {
    var _this = this;

    $.get('drawing.svg')
        .then(function (data) {
            var $svg1 = $(data.documentElement);
            var $container = $('#container');
            $container.append($svg1);
            $svg1[0].getElementById('btnLed1On').addEventListener('click', function () { _this.turnOn_led1(); });
            $svg1[0].getElementById('btnLed1Off').addEventListener('click', function () { _this.turnOff_led1(); });
        });

    $scope.greeting = { text: 'hello' };
    $scope.state = { sw1: false, led1: false };

    var conn = $.hubConnection();
    var hub = conn.createHubProxy("MyHub");

    hub.on("StateChange", function (state) {
        $scope.$apply(function () { $scope.state = state; });
    });

    conn.start()
    .then(function () { return hub.invoke('Enter'); })
    .then(function (state) {
        $scope.$apply(function () { $scope.state = state; });
    });

    this.turnOn_led1 = function () {
        hub.invoke('Turn', 'on', 'led1');
    };
    this.turnOff_led1 = function () {
        hub.invoke('Turn', 'off', 'led1');
    };
});