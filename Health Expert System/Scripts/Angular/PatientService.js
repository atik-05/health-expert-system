app.service('PatientService', ['$filter', '$http', function ($filter, $http) {

    var service = {};
    var allDerivedSymptom = [];


    service.getProvider = function () {
        return $http.get('/Suggestion/getServiceProviders');
    }


    $http.get('/Suggestion/GetDerivedSymptoms')
        .then(function (response) {
            allDerivedSymptom = response.data;
        });

    service.getSymptom = function () {
        return $http.get('/Suggestion/GetSymptoms');
    }

    service.isCurrentUserChild = function () {
        return $http.get('/Suggestion/IsCurrentUserChild');
    }

    service.getDerivedSymptom = function (symptomId) {
        var derivedSymptom = ($filter('filter')(allDerivedSymptom, { SymptomId: symptomId }));
        return derivedSymptom;
    }

    service.getSelectionHistory = function () {
        return $http.get('/Selection/GetSelectionHistory');
    }

    service.getDistanceFromLatLonInKm = function (lat1, lon1, lat2, lon2) {
        var R = 6371; // Radius of the earth in km
        var dLat = deg2rad(lat2 - lat1);  // deg2rad below
        var dLon = deg2rad(lon2 - lon1);
        var a =
          Math.sin(dLat / 2) * Math.sin(dLat / 2) +
          Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
          Math.sin(dLon / 2) * Math.sin(dLon / 2);

        var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        var d = R * c; // Distance in km
        return d;
    }

    function deg2rad(deg) {
        return deg * (Math.PI / 180);
    }

    return service;

}]);