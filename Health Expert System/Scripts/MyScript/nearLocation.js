app.controller('nearLocation', function ($scope, $http) {

    $scope.distance = '';
    $scope.driving = '';

    $scope.initMap = function () {
        var shahbag = { lat: 23.754906810414933, lng: 90.38758993148804 };

        var map = new google.maps.Map(document.getElementById('map'), {
            center: shahbag,
            zoom: 12
        });

        var marker = new google.maps.Marker({
            position: shahbag,
            map: map,
            title: "click to zoom"
        });

        var bogura = new google.maps.LatLng(23.740176192166185, 90.39364099502563);
        var farmgat = new google.maps.LatLng(23.754906810414933, 90.38758993148804);


        $scope.distance = google.maps.geometry.spherical.computeDistanceBetween(bogura, farmgat);

        var directionsService = new google.maps.DirectionsService();
        var directionsDisplay = new google.maps.DirectionsRenderer(
        {
            suppressMarkers: true,
            suppressInfoWindows: true
        });
        directionsDisplay.setMap(map);
        var request = {
            origin: google.maps.LatLng(23.740176192166185, 90.39364099502563),
            destination: google.maps.LatLng(23.754906810414933, 90.38758993148804),
            travelMode: google.maps.DirectionsTravelMode.DRIVING
        };
        directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                directionsDisplay.setDirections(response);
                $scope.driving = "The distance between the two points on the chosen route is: " + response.routes[0].legs[0].distance.text;
                //distance += "The aproximative driving time is: " + response.routes[0].legs[0].duration.text;
                //document.getElementById("distance_road").innerHTML = distance;
            }
        });
    }
});