app.controller('registrationController',
    function ($scope, $http) {

        $scope.loading = false;
        $scope.userNameExist = false;
        $scope.checkUser = true;

        $scope.PatientProfile = {
            UserName: '',
            Password: '',
            Name: '',
            Gender: '',
            Age: '',
            Occupation: '',
            MonthlyIncome: '',
            LifestyleStatus: ''
        };

        $scope.checkUser = function () {
            $scope.userNameExist = false;
            $http.get('/Registration/IsExistHospital?userName=' + $scope.PatientProfile.UserName)
                .then(function (response) {
                    if (response.data == "True") {
                        $scope.userNameExist = true;
                    } else {
                        $scope.checkUser = false;
                    }
                });
        }

        $scope.register = function () {
            $scope.loading = true;

            $http.post('/Registration/RegisterPatient', $scope.PatientProfile)
               .then(function (response) {
                   window.location = '/Home/Index';
               });
        }

        function sendToServer() {
            $http.post('/Registration/RegisterPatient', $scope.PatientProfile)
               .then(function (response) {
                   window.location = '/Registration/Index';
               });
        }
        

        
    });