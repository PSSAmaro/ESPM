var app = angular.module('GestaoESPM', []);

app.controller('PedidosCtrl', function ($scope, $http, $interval) {
    $scope.abertos = true;
    $scope.mensagem = "A carregar";
    $scope.carregado = false;
    $scope.url = '/gestao/api/pedidos';
    $scope.atualizar = function () {
        $http.get($scope.url).then(function (result) {
            $scope.pedidos = result.data;
            $scope.carregado = true;
        }, function (result) {
            $scope.mensagem = "Erro ao carregar";
        });
    }
    $scope.atualizar();
    $scope.index = function () {
        $scope.carregado = false;
        $scope.abertos = true;
        $scope.url = '/gestao/api/pedidos';
        $scope.atualizar;
    }
    $scope.fechados = function () {
        $scope.carregado = false;
        $scope.abertos = false;
        $scope.url = '/gestao/api/pedidos/fechados';
        $scope.atualizar;
    }
    $scope.atualizacao = $interval($scope.atualizar, 3000);
});

app.controller('EstadosCtrl', function ($scope, $http) {
    $scope.mensagem = "A carregar";
    $scope.carregado = false;
    $scope.enviado = false;
    $scope.novonome = "";
    $http.get('/gestao/api/estados').then(function (result) {
        $scope.estados = result.data.Estados;
        $scope.inicial = result.data.Inicial;
        $scope.cancelado = result.data.Cancelado;
        $scope.carregado = true;
    }, function (result) {
            $scope.mensagem = "Erro ao carregar";
    });
    $scope.enviar = function () {
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
            $scope.classe = "text-success";
            $scope.resultado = "Estados alterados com sucesso.";
        }, function (result) {
            $scope.classe = "text-danger";
            $scope.resultado = "Erro ao alterar os estados.";
        });
    }
    $scope.novo = function () {
        if ($scope.novonome.length !== 0) {
            dados = {
                'Nome': $scope.novonome
            }
            $http.post('/gestao/api/estados/novo', dados).then(function (result) {
                $scope.novonome = "";
                $scope.classenovo = "text-success";
                $scope.mensagemnovo = "Novo estado adicionado. Clique em Editar para o configurar.";
                $scope.estados.push(result.data);
            }, function (result) {
                $scope.classenovo = "text-danger";
                $scope.mensagemnovo = "Ocorreu um erro ao criar o novo estado.";
            });
        }
        else {
            $scope.classenovo = "text-danger";
            $scope.mensagemnovo = "Por favor escolha um nome para o novo estado.";
        }
    }
});