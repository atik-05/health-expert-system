app.controller('homeCtrl', ['$scope', '$http', function ($scope, $http) {

    $scope.notRatedProviders = [];
    $scope.recentHistory = [];
    $scope.ratingRange = [];
    $scope.userProfile = [];
    $scope.edit = false;
    $scope.editButton = true;
    $scope.saveButton = false;



    $http.get('/Selection/GetNotRatedProvider')
        .then(function (response) {
            $scope.notRatedProviders = response.data;

            getRecentHistory();
        });


    function getRecentHistory() {
        $http.get('/Selection/GetRecentTreatmentHistory')
                .then(function (response) {
                    $scope.recentHistory = response.data;
                });
    }

    $scope.storeRating = function (historyId, providerId, rating) {
        var params = {Id: historyId, ServiceProviderId: providerId, Rating: rating};
        $http.post('/Home/StoreRating', params)
            .then(function(response) {

            });
    }

    for (var i = 1; i < 11; i++) {
        $scope.ratingRange.push(i);
    }

    $http.get('/Home/GetUserProfile')
        .then(function(response) {
            $scope.userProfile = response.data;
        });

    $scope.editProfile = function() {
        $scope.edit = true;
        $scope.editButton = false;
        $scope.saveButton = true;
    }

    $scope.saveProfile = function() {
        $http.post('/Home/SaveProfile', $scope.userProfile)
            .then(function(response) {
                window.location.href = "/Home/MyProfile";
            });
    }


}]);