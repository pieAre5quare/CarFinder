(function () {
    angular.module('car-finder')

      .controller('carCtrl', ['carService', '$q', '$modal', function (carSvc, $q, $modal) {
          var self = this;
          self.selected = {
              year: '',
              make: '',
              model: '',
              trim: '',
              filter: '',
              paging: 'true',
              page: '0',
              perPage: '10',
              sortcolumn: 'id',
              sortByReverse: 'true'
          }

          self.options = {
              years: '',
              makes: '',
              models: '',
              trims: ''
          }

          self.cars = [];

          self.modalCar = '';

          self.unpagedCarsCount = 0;

          self.loading = false;

          self.getYears = function () {
              carSvc.getYears().then(function (data) {
                  self.options.years = data;
                  self.getCars();
                  self.getCarsCount();
              })
          }

          self.getMakes = function () {
              self.options.makes = '';
              self.selected.make = '';
              self.options.models = '';
              self.selected.model = '';
              self.options.trims = '';
              self.selected.trim = '';
              self.cars = [];

              carSvc.getMakes(self.selected).then(function (data) {
                  self.options.makes = data;
              })
              self.getCars();
          }

          self.getModels = function () {
              self.options.models = '';
              self.selected.model = '';
              self.options.trims = '';
              self.selected.trim = '';
              self.cars = [];

              carSvc.getModels(self.selected).then(function (data) {
                  self.options.models = data;
              })
              self.getCars();
          }

          self.getTrims = function () {
              self.options.trims = '';
              self.selected.trim = '';
              self.cars = [];

              carSvc.getTrims(self.selected).then(function (data) {
                  self.options.trims = data;
              })
              self.getCars();
          }

          self.getCars = function () {
              self.cars = [];
              if (!self.loading) {
                  self.loading = true;
                  var s = angular.copy(self.selected);
                  s.page += 1;
                  $q.all([carSvc.getCars(s), carSvc.getCarsCount(s)])
                    .then(function (data) {
                        self.cars = data[0];
                        self.unpagedCarsCount = data[1];
                        self.loading = false;
                    })
              }
          }

          self.getCarsCount = function () {
              carSvc.getCarsCount(self.selected).then(function (data) {
                  self.unpagedCarsCount = data;
              })
          }

          self.getCar = function () {
              carSvc.getCar(id).then(function (data) {
                  self.modalCar = data;
              })
          }

          self.open = function (id) {
              var modalInstance = $modal.open({
                  animation: true,
                  templateUrl: 'carModal.html',
                  controller: 'carModalCtrl as cm',
                  size: 'large',
                  resolve: {
                      car: function () {
                          return carSvc.getCar(id);
                      }
                  }


              });
          };

          self.getYears(self.selected);
      }]);

    angular.module('car-finder').controller('carModalCtrl', function ($modalInstance, car) {

        var scope = this;

        scope.car = car;

        scope.ok = function () {
            $modalInstance.close();
        };

        scope.cancel = function () {
            $modalInstance.dismiss();
        };
    })
})();