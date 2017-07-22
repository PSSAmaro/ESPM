using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    // Inicializador temporário
    public class DbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            // Inicializar definições
            db.Definicoes.AddRange(new List<Definicao>()
            {
                new Definicao()
                {
                    Nome = "PlataformaLigada",
                    Maximo = 1,
                    Valor = 1,
                    Apresentacao = "Receção de pedidos de ajuda",
                    Descricao = "Define se o sistema está a receber pedidos.",
                    SignificadoZero = "Não aceitar pedidos",
                    SignificadoOutro = "Aceitar pedidos",
                    Listar = true
                },
                new Definicao()
                {
                    Nome = "GestaoLigada",
                    Maximo = 1,
                    Valor = 1,
                    Apresentacao = "Plataforma de gestão",
                    Descricao = "Define se os operadores podem aceder ao módulo de gestão.",
                    SignificadoZero = "Não permitir acesso",
                    SignificadoOutro = "Permitir acesso",
                    Listar = true
                },
                new Definicao()
                {
                    Nome = "TesteLigado",
                    Maximo = 1,
                    Valor = 1,
                    Apresentacao = "Modo de teste",
                    Descricao = "Define se o modo de teste está ativo.",
                    SignificadoZero = "Modo de teste desligado",
                    SignificadoOutro = "Modo de teste ligado",
                    Listar = true
                },
                new Definicao()
                {
                    Nome = "ValidadeAutorizacao",
                    Maximo = 0,
                    Valor = 365,
                    Apresentacao = "Validade das autorizações",
                    Descricao = "Define a quantidade de dias que devem ser dados a uma nova autorização.",
                    SignificadoZero = "Sem validade (Não recomendado)",
                    SignificadoOutro = "Número de dias de autorização",
                    Listar = true
                },
                new Definicao()
                {
                    Nome = "ValidadeTeste",
                    Maximo = 0,
                    Valor = 365,
                    Apresentacao = "Validade das autorizações de teste",
                    Descricao = "Define a quantidade de dias que devem ser dados a uma nova autorização de teste.",
                    SignificadoZero = "Sem validade",
                    SignificadoOutro = "Número de dias de autorização",
                    Listar = true
                },
                new Definicao()
                {
                    Nome = "LimitePedidosFalsosAut",
                    Maximo = 0,
                    Valor = 20,
                    Apresentacao = "Limite de falsos pedidos por autorização",
                    Descricao = "Define a quantidade de pedidos falsos que podem ser recebidos antes de uma autorização ser revogada.",
                    SignificadoZero = "Limite desativado",
                    SignificadoOutro = "Limite de pedidos falsos",
                    Listar = true
                },
                new Definicao()
                {
                    Nome = "LimitePedidosFalsosIP",
                    Maximo = 0,
                    Valor = 5,
                    Apresentacao = "Limite de falsos pedidos por IP",
                    Descricao = "Define a quantidade de pedidos falsos que podem ser recebidos do mesmo IP antes de rejeitar o IP",
                    SignificadoZero = "Limite desativado",
                    SignificadoOutro = "Limite de pedidos falsos",
                    Listar = true
                },
                new Definicao()
                {
                    Nome = "BloqueioOperador",
                    Maximo = 1,
                    Valor = 1,
                    Apresentacao = "Bloquear pedidos ao operador",
                    Descricao = "Define se os pedidos devem ser associados a um operador, impedindo o tratamento do mesmo pedido por vários operadores.",
                    SignificadoZero = "Vários operadores podem tratar do mesmo pedido",
                    SignificadoOutro = "Apenas 1 operador pode tratar de cada pedido",
                    Listar = true
                },
                new Definicao()
                {
                    Nome = "EstadoInicial",
                    Maximo = 0,
                    Valor = 1,
                    Apresentacao = "Estado inicial",
                    Descricao = "Define o estado que deve ser atribuido aos novos pedidos recebidos.",
                    SignificadoZero = "Não permitido",
                    SignificadoOutro = "ID do estado",
                    Listar = false
                },
                new Definicao()
                {
                    Nome = "EstadoCancelado",
                    Maximo = 0,
                    Valor = 11,
                    Apresentacao = "Estado cancelado",
                    Descricao = "Define o estado que deve ser atribuido aos pedidos que foram cancelados por quem enviou.",
                    SignificadoZero = "Não permitido",
                    SignificadoOutro = "ID do estado",
                    Listar = false
                },
                new Definicao()
                {
                    Nome = "Familias",
                    Maximo = 0,
                    Valor = 8,
                    Apresentacao = "Número de famílias de estados",
                    Descricao = "Define a quantidade de famílias de estados existentes.",
                    SignificadoZero = "Não permitido",
                    SignificadoOutro = "Número de famílias",
                    Listar = false
                }
            });

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            // Criar roles: Admin, Operador e Aplicacao
            // Admin = Administrador(es) da plataforma
            RoleManager.Create(new IdentityRole("Admin"));
            // Operador = Operadores responsáveis pela gestão dos pedidos recebidos
            RoleManager.Create(new IdentityRole("Operador"));
            // Desautorizado = Antigos Admin/Operador, mantidos para questões de log, mas desautorizados
            RoleManager.Create(new IdentityRole("Desautorizado"));
            // Dev = Utilizadores de desenvolvimento, responsáveis pelas (suas) aplicações, com acesso ao modo de teste
            RoleManager.Create(new IdentityRole("Dev"));
            // ExDev = Antigos utilizadores de desenvolvimento, entretanto desautorizados
            RoleManager.Create(new IdentityRole("ExDev"));

            // Criar utilizador (APENAS TESTE!!!)
            var user = new ApplicationUser()
            {
                UserName = "teste@a.com"
            };
            UserManager.Create(user, "123456");

            // Adicionar utilizador à role Aplicacao
            UserManager.AddToRole(user.Id, "Dev");

            // Adicionar aplicação
            db.Aplicacoes.Add(new Aplicacao()
            {
                Utilizador = user,
                Nome = "Aplicação Teste",
                Autorizacoes = new List<Autorizacao>
                {
                    new Autorizacao()
                    {
                        Utilizador = user,
                        Validade = DateTime.Now.AddYears(1),
                        Teste = false,
                        Revogada = false
                    }
                }
            });

            List<Estado> Estados = new List<Estado>()
            {
                new Estado()
                {
                    Nome = "Alerta",
                    Familia = 1,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Análise",
                    Familia = 1,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Em Despacho",
                    Familia = 1,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Despacho 1º Alerta",
                    Familia = 2,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Chegada ao TO",
                    Familia = 3,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Em Curso",
                    Familia = 3,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Resolução",
                    Familia = 4,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Conclusão",
                    Familia = 5,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Falso Alerta",
                    Familia = 6,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Falso Alarme",
                    Familia = 6,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Anulada",
                    Familia = 6,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Vigilância",
                    Familia = 7,
                    Ativo = true
                },
                new Estado()
                {
                    Nome = "Encerrada",
                    Familia = 8,
                    Ativo = true
                }
            };

            db.Estados.AddRange(Estados);

            db.TransicoesDeEstado.Add(new TransicaoDeEstado
            {
                De = Estados[0],
                Para = Estados[1],
                Ativa = true
            });

            db.SaveChanges();

            base.Seed(db);
        }
    }
}