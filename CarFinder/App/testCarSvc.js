(function () {
    angular.module('car-finder')
        .factory('carSvc', ['$q', function ($q) {
            var f = {};
            var models =
            {
                "Mazda": ["3", "6", "RX-8"],
                "Chevy": ["Camaro", "Colorado", "Corvette"],
                "Ford": ["F-150", "F-250", "F-350"]
            }

            var trims =
                {
                    "Mazda":
                        {
                            "3": ["Sport", "Touring"],
                            "6": ["Sport", "Touring"],
                            "RX-8": ["Sport", "Grand Touring"]
                        },
                    "Chevy":
                        {
                            "Camaro": ["1LT", "2LT", "1SS", "2SS"],
                            "Colorado": ["Base", "WT", "LT", "Z71"],
                            "Corvette": ["Stingray", "Stingray Z51", "Z06"]
                        },

                    "Ford":
                        {
                            "F-150": ["XL", "XLT", "Lariat"],
                            "F-250": ["XL", "XLT", "Lariat"],
                            "F-350": ["XL", "XLT", "Lariat"]
                        }
                }

            f.getYears = function () {
                var d = $q.defer();
                d.resolve(['1999', '2000', '2001', '2002']);
                return d.promise;
            }

            f.getMakes = function (year) {
                var d = $q.defer();
                switch (year) {
                    case '1999':
                    case '2000':
                        d.resolve(['Mazda', 'Chevy']);
                        break;
                    case '2001':
                    case '2002':
                        d.resolve(['Chevy', 'Ford']);
                        break;
                }
                return d.promise;
            }

            f.getModels = function (year, make) {
                var d = $q.defer();
                d.resolve(models[make]);
                return d.promise;
            }

            f.getTrims = function (year, make, model) {
                var d = $q.defer();
                d.resolve(trims[make][model]);
                return d.promise;
            }

            f.getCars = function (year, make, model, trim) {
                var d = $q.defer();

                d.resolve([
                    {
                        year: year,
                        make: make,
                        model: model,
                        trim: trim,
                        color: 'black'
                    }, {
                        year: year,
                        make: make,
                        model: model,
                        trim: trim,
                        color: 'black'
                    }
                ]);
                return d.promise;
            }

            return f;
        }])
})();