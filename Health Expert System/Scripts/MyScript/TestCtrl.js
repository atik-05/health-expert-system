var myApp = angular.module('myApp', []);

myApp.controller('myCtrl', ['$scope', 'PatientService', function ($scope, PatientService) {

    
    $scope.result = '';
    $scope.okButton = true;

    $scope.disease = {
        symptomId: '',
        derivedSymptomId: 0
    };
    var promise = PatientService.getSymptom();
    promise.then(
        function(response) {
            $scope.symptoms = response.data;
        });

    //$scope.symptoms = PatientService.getSymptom();



    $scope.getDerivedSymptom = function () {
        $scope.okButton = true;

        $scope.result = PatientService.getDerivedSymptom($scope.disease.symptomId);

        if ($scope.result.length == 0) {
            $scope.result = '';
            $scope.okButton = false;
        }
    }

    $scope.complete = function () {
        $scope.okButton = false;
    }

}]);

myApp.service('PatientService', ['$filter', '$http', function ($filter, $http) {

    var service = {};
    var allDerivedSymptom = [];

    

    $http.get('/Location/GetDerivedSymptoms')
        .then(function (response) {
            allDerivedSymptom = response.data;
        });

    service.getSymptom = function () {
        
        //var symptoms = [];
        return $http.get('/Location/GetSymptoms');
            

        //return symptoms;
    }

    service.getDerivedSymptom = function(symptomId) {
        var derivedSymptom = ($filter('filter')(allDerivedSymptom, { SymptomId: symptomId }));
        return derivedSymptom;
    }

    return service;

}]);