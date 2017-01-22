var app = angular.module('mapApp', []);

app.controller('mapCtrl', function ($scope, $http) {

    $scope.showMap = true;
    $scope.presentLat = '';
    $scope.presentLong = '';
    $scope.clickedLat = '';
    $scope.clickedLng = '';

    var marker;

    
    
    var handleLocationError = function (browserHasGeolocation, infoWindow, pos) {
        infoWindow.setPosition(pos);
        infoWindow.setContent(browserHasGeolocation ?
                              'Error: The Geolocation service failed.' :
                              'Error: Your browser doesn\'t support geolocation.');
    }
    $scope.loadMap = function () {

        var map = new google.maps.Map(document.getElementById('sample'), {
            center: { lat: -34.397, lng: 150.644 },
            zoom: 14
        });

        google.maps.event.addDomListener(map,
            'click',
            function() {
                window.alert('Map was clicked!');
            });

        //var infoWindow = new google.maps.InfoWindow({ map: map });

        //Try HTML5 geolocation.
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };

                $scope.presentLat = pos.lat;
                $scope.presentLong = pos.lng;

                marker = new google.maps.Marker({
                    position: pos,
                    map: map,
                    title: 'Click to zoom'
                });

                //infoWindow.setPosition(pos);
                //infoWindow.setContent('Location found.');
                map.setCenter(pos);
            }, function () {
                handleLocationError(true, infoWindow, map.getCenter());
            });

            // click event 
            //google.maps.event.addListener(map, 'click', function (event) {
            //    placeMarker(event.latLng);

            //});
        } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, infoWindow, map.getCenter());
        }

        function placeMarker(location) {
            if (marker) {
                marker.setPosition(location);
            } else {
                marker = new google.maps.Marker({
                    position: location,
                    map: map
                });
            }
            
            //var infowindow = new google.maps.InfoWindow({
            //    content: 'Latitude: ' + location.lat() + '<br>Longitude: ' + location.lng()
            //});
            //infowindow.open(map, marker);

            $scope.clickedLat = location.lat();
            $scope.clickedLng = location.lng();
        }

        //google.maps.event.addDomListener(window, 'load', initialize);



        
    }
    
    $scope.addInfo = function () {
        $scope.showMap = false;
    }
        

});