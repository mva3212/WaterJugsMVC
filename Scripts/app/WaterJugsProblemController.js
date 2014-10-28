//Real simple angular app 
var app = angular.module('WaterJugsApp', ["ui.bootstrap"]);

var HomeController = function ($scope,$http) {
    $scope.model = { JugXCapacity: null, JugYCapacity: null, GoalCapacity: null, ShortestSolution: null, UnsolvableReason: null };
    $scope.alerts = [];

    //Close the Alerts on click
    $scope.closeAlert = function (index) { $scope.alerts.splice(index, 1); };
    
    $scope.solve = function (form, valid) {
        // Client-side validation requires valid number between 1-999999
        if (valid) {
            //Could be broken out to a service as application necessitates
            $http.post('/SolveJugProblem', { JugXCapacity: $scope.model.JugXCapacity, JugYCapacity: $scope.model.JugYCapacity, GoalCapacity: $scope.model.GoalCapacity }).success(function (data) {
                $scope.model = data;
            }).error(function(data) {
                $scope.alerts.push({ type: "danger", msg: "An Error has occured. Please contact your support desk to be treated like an imbecile and asked to reboot your computer. "});

            });
        } else {
            $scope.alerts = [];
            angular.forEach(form.$error.required, function(field) {
                $scope.alerts.push({  msg: field.$name + " is a required field" });
            });
            angular.forEach(form.$error.number, function (field) {
                $scope.alerts.push({ msg: field.$name + " must be a number." });
            });
            angular.forEach(form.$error.min, function (field) {
                $scope.alerts.push({ msg: field.$name + " must be greater than 0." });
            });
            angular.forEach(form.$error.max, function (field) {
                $scope.alerts.push({ msg: field.$name + " must be less than 999999." });
            });
        }
    };
}

HomeController.$inject = ['$scope', '$http'];

app.controller('HomeController', HomeController);
