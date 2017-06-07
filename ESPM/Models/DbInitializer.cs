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
                    Nome = "PlataformaAtiva",
                    Maximo = 1,
                    Valor = 1,
                    Descricao = "Define se o sistema está a receber pedidos.",
                    SignificadoZero = "Não aceitar pedidos",
                    SignificadoOutro = "Aceitar pedidos"
                },
                new Definicao()
                {
                    Nome = "GestaoAtiva",
                    Maximo = 1,
                    Valor = 1,
                    Descricao = "Define se os operadores podem aceder ao módulo de gestão.",
                    SignificadoZero = "Não permitir acesso",
                    SignificadoOutro = "Permitir acesso"
                },
                new Definicao()
                {
                    Nome = "TesteAtivo",
                    Valor = 1,
                    Descricao = "Define se o modo de teste está ativo.",
                    SignificadoZero = "Modo de teste desligado",
                    SignificadoOutro = "Modo de teste ligado"
                },
                new Definicao()
                {
                    Nome = "DemoAtivo",
                    Maximo = 1,
                    Valor = 1,
                    Descricao = "Define se a demonstração está ativa.",
                    SignificadoZero = "Demonstração indisponível",
                    SignificadoOutro = "Demonstração disponível"
                },
                new Definicao()
                {
                    Nome = "LimitePedidosFalsosApp",
                    Maximo = 0,
                    Valor = 10,
                    Descricao = "Define a quantidade de pedidos falsos que podem ser recebidos antes de uma app ser desativada.",
                    SignificadoZero = "Limite desativado",
                    SignificadoOutro = "Limite de pedidos falsos"
                },
                new Definicao()
                {
                    Nome = "LimitePedidosFalsosIP",
                    Maximo = 0,
                    Valor = 3,
                    Descricao = "Define a quantidade de pedidos falsos que podem ser recebidos do mesmo IP antes de rejeitar o IP",
                    SignificadoZero = "Limite desativado",
                    SignificadoOutro = "Limite de pedidos falsos"
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
            // Aplicacao = Utilizadores responsáveis pelas (suas) aplicações, com acesso ao modo de teste
            RoleManager.Create(new IdentityRole("Aplicacao"));

            // Criar utilizador (APENAS TESTE!!!)
            var user = new ApplicationUser()
            {
                UserName = "Teste"
            };
            UserManager.Create(user, "123456");

            // Adicionar utilizador à role Aplicacao
            UserManager.AddToRole(user.Id, "Aplicacao");

            // Adicionar aplicação
            db.Aplicacoes.Add(new Aplicacao()
            {
                Utilizador = user,
                Nome = "Aplicação Teste",
                Autorizacoes = new List<Autorizacao>
                {
                    new Autorizacao()
                    {
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