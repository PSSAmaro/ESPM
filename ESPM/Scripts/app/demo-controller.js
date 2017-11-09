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
        // Isto provavelmente não é recomendado com Angular e deve haver outra maneira...
        google.maps.event.addListener(this.mapa, 'click', clique);
    };
});

// https://stackoverflow.com/questions/17063000/ng-model-for-input-type-file
app.directive("lerimagem", [function () {
    return {
        scope: {
            lerimagem: "="
        },
        link: function (scope, element, attributes) {
            element.bind("change", function (changeEvent) {
                var reader = new FileReader();
                reader.onload = function (loadEvent) {
                    scope.$apply(function () {
                        scope.lerimagem = loadEvent.target.result;
                    });
                }
                reader.readAsDataURL(changeEvent.target.files[0]);
            });
        }
    }
}]);

app.controller('ESPMCtrl', function ($scope, Mapa, $http, $interval) {
    $scope.enviado = false;
    $scope.botao = "Enviar";
    $scope.pedido = "";
    $scope.modificado = "";
    $scope.dados = {
        'Localizacoes': [
            {
                'Latitude': 32.745,
                'Longitude': -16.968
            }
        ],
        'Imagens': []
    };
    $scope.chave = "5076b31b-d9bf-e711-a219-74de2b8eb05f";
    $scope.config = {
        'headers': {
            'X-ESPM-Autenticacao': $scope.chave
        }
    };
    $scope.consola = "Chave da demonstração: " + $scope.chave + "\n";
    $scope.clique = function (event) {
        $scope.dados.Localizacoes[0].Latitude = event.latLng.lat();
        $scope.dados.Localizacoes[0].Longitude = event.latLng.lng();
        Mapa.marcador.setPosition(event.latLng);
        Mapa.mapa.panTo(event.latLng);
        $scope.$apply();
    };
    $scope.enviar = function () {
        $http.post('/api/emergencia', $scope.dados, $scope.config).then(function (result) {
            $scope.enviado = true;
            $scope.botao = "Atualizar";
            $scope.pedido = result.data.Id;
            $scope.consola += "ID do pedido: " + result.data.Id + "\n";
            $scope.atualizacao = $interval($scope.estado, 3000);
            delete $scope.dados.Imagens[0];
        }, function (result) {
            $scope.consola += result.data.Message + "\n";
        });
    };
    $scope.atualizar = function () {
        $http.put('/api/emergencia/' + $scope.pedido, $scope.dados, $scope.config).then(function (result) {
            $scope.consola += "Detalhes atualizados? " + result.data.Atualizado + "\n";
            delete $scope.dados.Imagens[0];
        }, function (result) {
            $scope.consola += result.data.Message + "\n";
        });
    };
    $scope.cancelar = function () {
        $http.delete('/api/emergencia/' + $scope.pedido, $scope.dados, $scope.config).then(function (result) {
            $scope.consola += "Pedido de ajuda cancelado\n";
            $scope.enviado = false;
            $scope.botao = "Enviar";
            $interval.cancel($scope.atualizacao);
        }, function (result) {
            $scope.consola += result.data.Message + "\n";
        });
    };
    $scope.estado = function () {
        $http.get('/api/emergencia/' + $scope.pedido).then(function (result) {
            if (result.data.Modificado !== $scope.modificado) {
                $scope.consola += "(" + new Date().toLocaleTimeString() + ") Estado atual: " + result.data.Estado + "\n";
                $scope.modificado = result.data.Modificado;
            }
        }, function (result) {
            $scope.consola += result.data.Message + "\n";
        });
    };
    Mapa.init({ lat: $scope.dados.Localizacoes[0].Latitude, lng: $scope.dados.Localizacoes[0].Longitude }, $scope.clique);
});