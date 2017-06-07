var app = angular.module('ESPM', []);

app.service('Mapa', function ($q) {
    this.init = function (posicao, clique) {
        this.mapa = new google.maps.Map(document.getElementById('map'), {
            zoom: 11,
            center: posicao
        });
        this.marcador = new google.maps.Marker({
            position: posicao,
            map: this.mapa
        });
        // Isto provavelmente não é recomendado com Angular e há outra maneira...
        google.maps.event.addListener(this.mapa, 'click', clique);
    }
});

app.controller('ESPMCtrl', function ($scope, Mapa, $http) {
    $scope.chave = "0123";
    $scope.posicao = { lat: 32.745, lng: -16.968 };
    $scope.consola = "Chave da demonstração: " + $scope.chave;
    $scope.clique = function (event) {
        $scope.posicao = { lat: event.latLng.lat(), lng: event.latLng.lng() };
        Mapa.marcador.setPosition(event.latLng);
        Mapa.mapa.panTo(event.latLng);
        $scope.$apply();
    }
    Mapa.init($scope.posicao, $scope.clique);
});