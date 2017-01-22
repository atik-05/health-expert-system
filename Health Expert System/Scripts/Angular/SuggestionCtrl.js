app.controller('suggestionCtrl', ['$scope', '$http', 'PatientService', '$q', function ($scope, $http, PatientService, $q) {

    $scope.result = '';
    $scope.isSelected = false;
    $scope.isCompleteProcess = false;
    $scope.isDecide = false;
    $scope.showResult = false;
    $scope.isCurrentUserChild = false;
    $scope.origin = { lat: '', lng: '' };
    $scope.serviceProviders = [];
    $scope.location = '';
    $scope.detectLocation = true;
    $scope.showMap = false;
    $scope.showMode = true;

    $scope.doctors = [];

    var providerCount = 0;
    var count = 0;

    $scope.requiredData = {
        SymptomId: 0,
        DerivedSymptomId: 0,
        IsEmergency: false,
        SuggestionTypeId: 1,
        AllDistances: [{
            Id: '',
            Distance: ''
        }]
    };

    getAddress();

    var childTestPromise = PatientService.isCurrentUserChild()
        .then(function (response) {
            var isCild = response.data;
            if (isCild == true) {
                $scope.isCurrentUserChild = true;
            }
            //$scope.isDecide = true;
        });

    $scope.confirmLocation = function () {
        $scope.detectLocation = false;
        $scope.isDecide = true;
    }

    $scope.editLocation = function () {
        $scope.showMap = true;

        var map = new google.maps.Map(document.getElementById('selectLocation'), {
            center: {
                lat: 12.9715987,
                lng: 77.59456269999998
            },
            zoom: 12
        });
        var marker;

        //search start
        var searchBox = new google.maps.places.SearchBox(document.getElementById('pac-input'));
        map.controls[google.maps.ControlPosition.TOP_CENTER].push(document.getElementById('pac-input'));
        google.maps.event.addListener(searchBox, 'places_changed', function () {
            searchBox.set('map', null);
            if (marker) {
                marker.setMap(null);
            }

            var places = searchBox.getPlaces();

            var bounds = new google.maps.LatLngBounds();
            var i, place;
            for (i = 0; place = places[i]; i++) {
                (function (place) {
                    marker = new google.maps.Marker({

                        position: place.geometry.location
                    });
                    marker.bindTo('map', searchBox, 'map');
                    google.maps.event.addListener(marker, 'map_changed', function () {
                        if (!this.getMap()) {
                            this.unbindAll();
                        }
                    });
                    bounds.extend(place.geometry.location);
                }(place));
                //cant find latlng from search
            }
            map.fitBounds(bounds);
            searchBox.set('map', map);
            map.setZoom(Math.min(map.getZoom(), 12));
        });
        //search end

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                marker = new google.maps.Marker({
                    position: pos,
                    map: map
                });
                map.setCenter(pos);

            });
        }
        google.maps.event.addListener(map, 'click', function (event) {
            if (marker) {
                marker.setMap(null);
            }
            marker = new google.maps.Marker({ position: event.latLng, map: map });
            var origin = {
                lat: event.latLng.lat(),
                lng: event.latLng.lng()
            };
            $scope.origin = origin;
        });
    }

    childTestPromise.then(function () {
        PatientService.getSymptom()
        .success(function (data) {
            $scope.symptoms = data;
        });
    });

    $scope.suggestion = function () {

        $http.post('/Suggestion/GetPatientData', $scope.requiredData)
            .success(function (data) {
                $scope.isDecide = false;
                $scope.showResult = true;
                $scope.doctors = data;
                console.log("mission accomplished..");
            });
    }

    $scope.getDerivedSymptom = function () {
        $scope.isSelected = false;

        $scope.result = PatientService.getDerivedSymptom($scope.requiredData.SymptomId);

        if ($scope.result.length == 0) {
            $scope.result = '';
            $scope.isSelected = true;
        }
    }

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

    $scope.complete = function () {
        $scope.isSelected = true;
    }

    $scope.selectProvider = function (id) {
        var params = { id: id, diseaseId: $scope.requiredData.SymptomId, latitude: $scope.origin.lat, longitude: $scope.origin.lng };
        $http.post('/Selection/SelectServiceProvider/', params)
            .then(function (response) {
                console.log("selection complete");
                window.location.href = "/Home/Index";
                //alert("selection ok");
            });
    }

    $scope.checkEmergency = function() {
        if ($scope.requiredData.IsEmergency == true) {
            $scope.showMode = false;
            $scope.suggestionMode();
        } else {
            $scope.showMode = true;
        }
    }

    $scope.suggestionMode = function () {
        console.log("another mode");
        if ($scope.requiredData.SuggestionTypeId == 1) {
            PatientService.getProvider()
        .success(function (data) {

            console.log("service provider get ok");
            $scope.serviceProviders = data;

            providerCount = $scope.serviceProviders.length;
            for (var i = 0; i < providerCount; i++) {
                var id = $scope.serviceProviders[i].Id;
                var distance = PatientService.getDistanceFromLatLonInKm($scope.origin.lat, $scope.origin.lng, $scope.serviceProviders[i].Latitude, $scope.serviceProviders[i].Longitude);
                distance = Math.round(distance * 1000) / 1000;
                var ob = {
                    Id: id,
                    Distance: distance
                };
                $scope.requiredData.AllDistances.push(ob);
                count++;
                console.log(ob);

                if (count == providerCount) {
                    $scope.isCompleteProcess = true;
                    console.log($scope.isCompleteProcess);
                }

            }
        });
        } else {
            var histories = [];
            PatientService.getSelectionHistory()
                .success(function (data) {

                    console.log("service provider get ok");
                    histories = data;
                    providerCount = histories.length;
                    for (var i = 0; i < providerCount; i++) {
                        var id = histories[i].Id;
                        var distance = PatientService.getDistanceFromLatLonInKm($scope.origin.lat, $scope.origin.lng, histories[i].PatientLat, histories[i].PatientLng);
                        distance = Math.round(distance * 1000) / 1000;
                        var ob = {
                            Id: id,
                            Distance: distance
                        };
                        $scope.requiredData.AllDistances.push(ob);
                        count++;
                        console.log(ob);

                        if (count == providerCount) {
                            $scope.isCompleteProcess = true;
                            console.log($scope.isCompleteProcess);
                        }

                    }

                });
        }
    }

    function getAddress() {
        navigator.geolocation.getCurrentPosition(function (position) {
            var origin = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            $scope.origin = origin;
            var latlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({
                'latLng': latlng
            }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                                  
                        $scope.$apply(function() {
                            $scope.location = results[0].formatted_address;
                        });
                        console.log(results[0].formatted_address);
                    }
                }
            });

        });

    }

    $scope.showDirection = function(address) {
        var map = new google.maps.Map(document.getElementById('mapDirection'), {
            center: { lat: -34.397, lng: 150.644 },
            zoom: 14
        });
        var latitude = '';
        var longitude = '';
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode( { 'address': address}, function(results, status) {

            if (status == google.maps.GeocoderStatus.OK) {
                latitude = results[0].geometry.location.lat();
                longitude = results[0].geometry.location.lng();
            }

            var start = new google.maps.LatLng($scope.origin.lat, $scope.origin.lng);
            var end = new google.maps.LatLng(latitude, longitude);

            var directionsDisplay = new google.maps.DirectionsRenderer();
            directionsDisplay.setMap(map); 

            var request = {
                origin: start,
                destination: end,
                travelMode: google.maps.TravelMode.DRIVING
            };
            var directionsService = new google.maps.DirectionsService();
            directionsService.route(request, function (response, status2) {
                if (status2 == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                }
            });
        });
    }
}]);

