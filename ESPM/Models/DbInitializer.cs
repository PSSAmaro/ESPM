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
    public class DbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            // Criar role Aplicacao
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
                        Teste = false,
                        Revogada = false
                    }
                }
            });

            // Adicionar estado inicial
            db.Estados.Add(new Estado()
            {
                Nome = "Alerta"
            });

            db.SaveChanges();

            base.Seed(db);
        }
    }
}