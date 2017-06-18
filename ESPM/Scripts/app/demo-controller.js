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
    };
});

app.controller('ESPMCtrl', function ($scope, Mapa, $http, $interval) {
    $scope.pedido = "";
    $scope.modificado = "";
    $scope.dados = {
        'Aplicacao': '098ebeef-6154-e711-a1f4-74de2b8eb05f',
        'Localizacoes': [
            {
                'Latitude': 32.745,
                'Longitude': -16.968
            }
        ]
    };
    $scope.chave = "Chave temporariamente desativada";
    $scope.consola = "Chave da demonstração: " + $scope.chave + "\n";
    $scope.clique = function (event) {
        $scope.dados.Localizacoes[0].Latitude = event.latLng.lat();
        $scope.dados.Localizacoes[0].Longitude = event.latLng.lng();
        Mapa.marcador.setPosition(event.latLng);
        Mapa.mapa.panTo(event.latLng);
        $scope.$apply();
    };
    $scope.enviar = function () {
        $http.post('/api/emergencia', $scope.dados).then(function (result) {
            $scope.pedido = result.data.Id;
            $scope.consola += "ID do pedido: " + result.data.Id + "\n";
            $interval($scope.estado, 5000);
        }, function (result) {
            $scope.consola += result.data.Message + "\n";
        });
    };
    $scope.estado = function () {
        $http.get('/api/emergencia/' + $scope.pedido).then(function (result) {
            if (result.data.Modificado != $scope.modificado) {
                $scope.consola += "(" + new Date().toLocaleTimeString() + ") Estado atual: " + result.data.Estado + "\n";
                $scope.modificado = result.data.Modificado;
            }
        }, function (result) {
            $scope.consola += result.data.Message + "\n";
        });
    };
    Mapa.init({ lat: $scope.dados.Localizacoes[0].Latitude, lng: $scope.dados.Localizacoes[0].Longitude }, $scope.clique);
});