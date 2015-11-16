var app = angular.module('car-finder', []).controller('IndexController',
    ['$interval', '$timeout', function ($interval, $timeout) {
        var self = this;
        self.value = 1;
        $interval(function () {
            self.time = new Date();
        }, 100);

        self.fname = "Allan";
        self.lname = "Clark";

        self.currentfname = 0;
        self.currentlname = 0;

        self.prevName = function () {
            if (self.currentfname > 0) {
                self.currentfname--;
                self.currentlname--;
            } else {
                self.currentfname = self.fnames.length - 1;
                self.currentlname = self.lnames.length - 1;
            }
        }

        self.nextName = function () {
            if (self.currentfname < self.fnames.length - 1) {
                self.currentfname++;
                self.currentlname++;
            } else {
                self.currentfname = 0;
                self.currentlname = 0;
            }
        }

        self.lnames = ["Clark", "McNeill", "Smith"];
        self.fnames = ["Allan", "Tara", "John"];
    }]
    );