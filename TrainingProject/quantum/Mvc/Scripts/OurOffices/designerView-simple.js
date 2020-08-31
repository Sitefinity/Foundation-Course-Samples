(function () {
    angular.module('designer').requires.push('sfSelectors');
    angular.module('designer').controller('SimpleCtrl', ['$scope', 'propertyService', 'serverData', function ($scope, propertyService, serverData) {

        $scope.officeSelector = { selectedItemsIds: [] };

        $scope.$watchCollection('officeSelector.selectedItemsIds', function (officesIds) {
            if (typeof $scope.properties !== 'undefined' && typeof $scope.properties.StringifiedIds !== 'undefined') {
                $scope.properties.StringifiedIds.PropertyValue = JSON.stringify(officesIds);
            }
        });

        propertyService.get()
            .then(function (data) {
                if (data) {
                    $scope.properties = propertyService.toAssociativeArray(data.Items);

                    var serializedSelectedIds = $scope.properties.StringifiedIds.PropertyValue;
                    if (serializedSelectedIds !== "")
                        $scope.officeSelector.selectedItemsIds = JSON.parse(serializedSelectedIds);
                }
            });
    }]);
})();