app.controller('hospitalController',
    function($scope, $http) {

        $scope.showMap = true;
        $scope.form = false;
        $scope.loading = false;
        $scope.userNameExist = false;
        $scope.checkHospital = true;
        $scope.specialists = [];
        var selectedId = [];

        $scope.HospitalProfile = {
            UserName: '',
            Password: '',
            Name: '',
            Address: '',
            Type: 'Hospital',
            Latitude: '',
            Longitude: '',
            HospitalTypeId: '',
            ListOfSpecialistId: '',
            CabinFee: '',
            IsGovt: false,
            Rating: 0
        };

        $scope.loadMap = function () {

            var map = new google.maps.Map(document.getElementById('selectLocation'), {
                center: { lat: -34.397, lng: 150.644 },
                zoom: 14
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

                    $scope.HospitalProfile.Latitude = position.coords.latitude;
                    $scope.HospitalProfile.Longitude = position.coords.longitude;
                });
            }
            var geocoder = new google.maps.Geocoder();
            google.maps.event.addListener(map, 'click', function (event) {
                if (marker) {
                    marker.setMap(null);
                }
                geocoder.geocode({
                    'latLng': event.latLng
                }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                            $scope.HospitalProfile.Address = results[0].formatted_address;
                            //alert(results[0].formatted_address);
                        }
                    }
                });
                marker = new google.maps.Marker({ position: event.latLng, map: map });
                $scope.HospitalProfile.Latitude = event.latLng.lat();
                $scope.HospitalProfile.Longitude = event.latLng.lng();
                //alert("Latitude: " + event.latLng.lat() + "\r\nLongitude: " + event.latLng.lng());
            });

        }

        $scope.fillForm = function () {
            $scope.showMap = false;
            $scope.form = true;
        }
        $scope.checkUser = function () {
            $scope.userNameExist = false;
            $http.get('/Registration/IsExistHospital?userName=' + $scope.HospitalProfile.UserName)
                .then(function (response) {
                    if (response.data == "True") {
                        $scope.loading = false;
                        $scope.userNameExist = true;
                    } else {
                        $scope.checkHospital = false;
                    }
                });
        }
        $scope.register = function () {
            $scope.loading = true;
            converStringOfSpecialistId();

            $http.post('/Registration/RegisterHospital', $scope.HospitalProfile)
            .then(function (response) {
                window.location = '/Registration/Index';
            });
        }

        $http.get('/Registration/GetSpecialists')
            .then(function(response) {
                $scope.specialists = response.data;
            });

        $scope.addSpecialist = function(isChecked, id) {
            if (isChecked) {
                selectedId.push(id);
            } else {
                var index = selectedId.indexOf(id);
                selectedId.splice(index);
            }
        }

        function converStringOfSpecialistId() {
            for (var i = 0; i < selectedId.length; i++) {
                if (i==(selectedId.length-1)) {
                    $scope.HospitalProfile.ListOfSpecialistId += selectedId[i];
                } else {
                    $scope.HospitalProfile.ListOfSpecialistId += selectedId[i] + ",";
                }
                
            }
        }

    });