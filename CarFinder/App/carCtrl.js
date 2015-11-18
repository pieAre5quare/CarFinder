(function () {
    angular.module('car-finder')

      .controller('carCtrl', ['carService', function (carSvc) {
          var self = this;
          self.selected = {
              year: '',
              make: '',
              model: '',
              trim: '',
              filter: '',
              paging: '',
              page: '',
              perPage: '',
              sortcolumn: '',
              sortdirection: ''
          }

          self.options = {
              years: '',
              makes: '',
              models: '',
              trims: ''
          }

          self.cars = [];

          self.getYears = function () {
              carSvc.getYears().then(function (data) {
                  self.options.years = data;
                  //self.getCars(self.selected);
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
              //self.getCars(self.selected);
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
              //self.getCars(self.selected);
          }

          self.getTrims = function () {
              self.options.trims = '';
              self.selected.trim = '';
              self.cars = [];

              carSvc.getTrims(self.selected).then(function (data) {
                  self.options.trims = data;
              })
              self.getCars(self.selected);
          }

          self.getCars = function () {
              self.cars = [];
              carSvc.getCars(self.selected)
                  .then(function (data) {
                      self.cars = data;
                  })
          }

          self.getYears(self.selected);
      }])
})();