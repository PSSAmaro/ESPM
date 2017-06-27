using ESPM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ESPM.Helpers
{
    /// <summary>
    /// Gere as definições da aplicação
    /// </summary>
    public static class Definicoes
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Lista as definições do sistema que devem aparecer na página de definições.
        /// </summary>
        /// <returns>Definições em que Listar é True</returns>
        public static IQueryable<Definicao> Ler()
        {
            return db.Definicoes.Where(d => d.Listar == true);
        }

        /// <summary>
        /// Lê o valor de uma definição.
        /// </summary>
        /// <param name="def">Nome da definição.</param>
        /// <returns>Valor da definição.</returns>
        public static int Ler(string def)
        {
            List<Definicao> query = db.Definicoes.Where(d => d.Nome == def).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault().Valor;
            else
                return 0;
        }

        /// <summary>
        /// Altera o valor de uma definição.
        /// </summary>
        /// <param name="def">Definição a alterar.</param>
        /// <param name="val">Novo valor.</param>
        /// <returns>Sucesso da operação.</returns>
        public static bool Alterar(string def, int val)
        {
            // Garantir que a definição existe
            List<Definicao> query = db.Definicoes.Where(d => d.Nome == def).ToList();
            if (query.Count == 0)
                return false;
            Definicao definicao = query.FirstOrDefault();

            // Garantir que o valor é válido
            if (val < 0 || val > definicao.Maximo)
                return false;

            // Registar a alteração
            AlteracaoDefinicao alteracao = new AlteracaoDefinicao()
            {
                // https://stackoverflow.com/questions/20925822/asp-net-mvc-5-identity-how-to-get-current-applicationuser
                Utilizador = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.Current.User.Identity.GetUserId()),
                Definicao = definicao,
                De = definicao.Valor,
                Para = val,
                Tempo = DateTime.Now
            };
            db.AlteracoesDefinicoes.Add(alteracao);

            // Alterar a definição
            definicao.Valor = val;
            db.Entry(definicao).State = EntityState.Modified;

            // Guardar as alterações
            db.SaveChanges();

            return true;
        }
    }
}