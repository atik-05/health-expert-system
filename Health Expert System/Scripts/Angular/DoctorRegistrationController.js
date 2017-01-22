app.controller('doctorController',
    function ($scope, $http) {
        $scope.showMap = true;
        $scope.form = false;
        $scope.loading = false;
        $scope.userNameExist = false;
        $scope.invalidRegister = false;
        $scope.specialists = [];
        $scope.startTime = '';
        $scope.endTime = '';
        $scope.checkDoctor = true;

        $scope.DoctorProfile = {
            RegistrationNumber: '',
            UserName: '',
            Password: '',
            Name: '',
            Type: 'Doctor',
            Address: '',
            Latitude: '',
            Longitude: '',
            SpecialistId: '',
            YearOfExperience: '',
            VisitingFee: '',
            StartAndEndTime: '',
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

                    $scope.DoctorProfile.Latitude = position.coords.latitude;
                    $scope.DoctorProfile.Longitude = position.coords.longitude;
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
                            $scope.DoctorProfile.Address = results[0].formatted_address;
                            //alert(results[0].formatted_address);
                        }
                    }
                });
                marker = new google.maps.Marker({ position: event.latLng, map: map });
                $scope.DoctorProfile.Latitude = event.latLng.lat();
                $scope.DoctorProfile.Longitude = event.latLng.lng();
                //alert("Latitude: " + event.latLng.lat() + "\r\nLongitude: " + event.latLng.lng());
            });

        }

        $scope.fillForm = function () {
            $scope.showMap = false;
            $scope.form = true;
        }
        $scope.checkUser = function () {
            $scope.invalidRegister = false;
            $scope.userNameExist = false;

            if ($scope.DoctorProfile.RegistrationNumber > 0 && $scope.DoctorProfile.RegistrationNumber<99999) {
                $http.get('/Registration/Check?id=' + $scope.DoctorProfile.RegistrationNumber)
                .then(function (response) {
                    if (response.data == "True") {
                        $scope.invalidRegister = true;
                    }
                    if ($scope.invalidRegister == false && $scope.userNameExist == false) {
                        $scope.checkDoctor = false;
                    }
                });
            } else {
                $scope.invalidRegister = true;
            }
            
            $http.get('/Registration/IsExistUser?userName=' + $scope.DoctorProfile.UserName+'&&registrationNo='+ $scope.DoctorProfile.RegistrationNumber)
                .then(function (response) {
                    if (response.data == "True") {
                        $scope.loading = false;
                        $scope.userNameExist = true;
                    }
                    if ($scope.invalidRegister == false && $scope.userNameExist == false) {
                        $scope.checkDoctor = false;
                    }
                });
        }
        $scope.register = function () {
            $scope.loading = true;

            $http.post('/Registration/RegisterDoctor', $scope.DoctorProfile)
            .then(function (response) {
                window.location = '/Registration/Index';
            });
        }

        $http.get('/Registration/GetSpecialists')
            .then(function(response) {
                $scope.specialists = response.data;
            });

        $scope.setTimeFormat = function () {
            $scope.DoctorProfile.StartAndEndTime = $scope.startTime + " to " + $scope.endTime;
        }


    });