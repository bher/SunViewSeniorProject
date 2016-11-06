//Please add ng-app="myRouting" and ng-controller="ctrlRouting" to an element to make this work
    var a1 = angular.module('myRouting', [])
a1.controller('ctrlRouting', function ($scope, $http) {
    $scope.checkIP = function () {

        $http.post('not_Default.aspx/checkIP', {})
            .success(function (data, status) {
                if (data.d == "false") {
                    window.location.href = "index.html";
                }
            })
            .error(function (data, status) {
                $scope.status = status;
            });
    }
    $scope.logout = function () {
        $http.post('not_Default.aspx/logout', {})
            .success(function (data, status) {
                window.location.href = "index.html";
            })
            .error(function (status) {
            });
    };
    $scope.checkIP();
}); 