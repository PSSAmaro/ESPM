var app = angular.module('GestaoESPM', []);

app.controller('EstadosCtrl', function ($scope, $http) {
    $scope.mensagem = "A carregar";
    $scope.carregado = false;
    $scope.enviado = false;
    $http.get('/gestao/api/estados').then(function (result) {
        $scope.estados = result.data.Estados;
        $scope.inicial = result.data.Inicial;
        $scope.cancelado = result.data.Cancelado;
        $scope.carregado = true;
    }, function (result) {
            $scope.mensagem = "Erro ao carregar";
    });
    $scope.enviar = function ()
    {
        dados = {
            'Ativos': [],
            'Inicial': $scope.inicial,
            'Cancelado': $scope.cancelado
        };
        $scope.estados.forEach(function (e) {
            if (e.Ativo === true)
                dados['Ativos'].push(e.Id);
        });
        $http.post('/gestao/api/estados/editar', dados).then(function (result) {
            $scope.resultado = "Estados alterados com sucesso.";
            $scope.enviado = true;
        }, function (result) {
            $scope.resultado = "Erro ao alterar os estados.";
            $scope.enviado = true;
        });
    }
});