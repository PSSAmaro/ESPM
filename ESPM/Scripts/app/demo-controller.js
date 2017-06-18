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
    $scope.dados = {
        'Aplicacao': '083b7150-9050-e711-a1f4-74de2b8eb05f',
        'Localizacoes': [
            {
                'Latitude': 32.745,
                'Longitude': -16.968
            }
        ]
    };
    $scope.chave = "0123";
    $scope.consola = "Chave da demonstração: " + $scope.chave + "\n";
    $scope.clique = function (event) {
        $scope.dados.Localizacoes[0].Latitude = event.latLng.lat();
        $scope.dados.Localizacoes[0].Longitude = event.latLng.lng();
        Mapa.marcador.setPosition(event.latLng);
        Mapa.mapa.panTo(event.latLng);
        $scope.$apply();
    }
    $scope.enviar = function () {
        $http.post('/api/emergencia', $scope.dados, {
            headers: {
                'Hash': '48bc6539d9fac0ddf0fd93b26a98b787c67533c0'
            },
        }).then(function (result) {
            $scope.consola += "aaa";
        }, function (result) {
            $scope.consola += result.data.Message + "\n";
        });
    }
    Mapa.init({ lat: $scope.dados.Localizacoes[0].Latitude, lng: $scope.dados.Localizacoes[0].Longitude }, $scope.clique);
});