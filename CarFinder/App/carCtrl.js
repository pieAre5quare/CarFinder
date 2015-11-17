(function () {
    angular.module('car-finder')

      .controller('carCtrl', ['carSvc', function (carSvc) {
          var self = this;
          self.selected = {
              year: '',
              make: '',
              model: '',
              trim: ''
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
                  self.getCars();
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
              
              carSvc.getMakes(self.selected.year).then(function (data) {
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

              carSvc.getModels(self.selected.year, self.selected.make).then(function (data) {
                  self.options.models = data;
              })
              self.getCars();
          }

          self.getTrims = function () {
              self.options.trims = '';
              self.selected.trim = '';
              self.cars = [];

              carSvc.getTrims(self.selected.year, self.selected.make, self.selected.model).then(function (data) {
                  self.options.trims = data;
              })
              self.getCars();
          }

          self.getCars = function () {
              self.cars = [];
              carSvc.getCars(self.selected.year, self.selected.make, self.selected.model, self.selected.trim)
                  .then(function (data) {
                      self.cars = data;
                  })
          }

          self.getYears();
      }])
})();