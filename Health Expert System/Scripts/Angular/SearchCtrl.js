app.controller('searchCtrl', ['$scope', '$http', 'PatientService', '$q', function($scope, $http, PatientService, $q) {

    var providerCount = 0;
    var count = 0;
    var allDistances = [
        {
            Id: '',
            Distance: ''
        }
    ];
    $scope.providerDatas = [];

    $scope.test = "this is a test";

    PatientService.getProvider()
        .success(function (data) {

            console.log("service provider get ok");
            $scope.serviceProviders = data;
            var promise = setPresentLocation();

            promise.then(function () {

                providerCount = $scope.serviceProviders.length;
                for (var i = 0; i < providerCount; i++) {
                    var id = $scope.serviceProviders[i].Id;
                    var distance = PatientService.getDistanceFromLatLonInKm($scope.origin.lat, $scope.origin.lng, $scope.serviceProviders[i].Latitude, $scope.serviceProviders[i].Longitude);
                    distance = Math.round(distance * 1000) / 1000;
                    var ob = {
                        Id: id,
                        Distance: distance
                    };
                    allDistances.push(ob);
                    count++;
                    console.log(ob);

                    if (count == providerCount) {
                        $scope.isCompleteProcess = true;
                        console.log($scope.isCompleteProcess);
                        getProvidersData();
                    }

                }

            });
        });

    function setPresentLocation() {
        var defer = $q.defer();
        navigator.geolocation.getCurrentPosition(function (position) {
            var origin = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            console.log("present location set ok");
            $scope.origin = origin;
            defer.resolve();
        });
        return defer.promise;
    }

    function getProvidersData() {
        $http.post('/Home/GetProvidersData', allDistances)
            .then(function(response) {
                $scope.providerDatas = response.data;
            });
    }

}]);