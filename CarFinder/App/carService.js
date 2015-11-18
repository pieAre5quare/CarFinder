(function () {
    angular.module('car-finder')
        .factory('carService', ['$http', function ($http) {
            var f = {};

            f.getYears = function () {
                return $http.post('api/Cars/GetAllYears').then(function (response) {
                    return response.data;
                });
            }

            f.getMakes = function (selected) {
                return $http.post('api/Cars/GetMakeByYear', selected).then(function (response) {
                    return response.data;
                });
            }

            f.getModels = function (selected) {
                return $http.post('api/Cars/GetModelsByYearMake', selected).then(function (response) {
                    return response.data;
                });
            }

            f.getTrims = function (selected) {
                return $http.post('api/Cars/GetTrimsByYearMakeModel', selected).then(function (response) {
                    return response.data;
                });
            }

            f.getCars = function (selected) {
                return $http.post('api/Cars/GetCars', selected).then(function (response) {
                    return response.data;
                });
            }



            return f;
        }])
})();