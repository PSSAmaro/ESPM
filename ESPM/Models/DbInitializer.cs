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
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            // Criar roles: Admin, Operador e Aplicacao
            RoleManager.Create(new IdentityRole("Admin"));
            RoleManager.Create(new IdentityRole("Operador"));
            RoleManager.Create(new IdentityRole("Aplicacao"));

            // Criar utilizador
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