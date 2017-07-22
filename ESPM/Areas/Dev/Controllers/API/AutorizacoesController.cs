using ESPM.Areas.Dev.Models;
using ESPM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ESPM.Areas.Dev.Controllers.API
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "Dev")]
    [RoutePrefix("Dev/api/Autorizacoes")]
    public class AutorizacoesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route]
        public IHttpActionResult Get()
        {
            // Carregar todas as autorizações do utilizador atual que não tenham sido revogadas (umas vez que estas tecnicamente já não existem)
            IQueryable<Autorizacao> autorizacoes = db.Autorizacoes.Where(a => !a.Revogada && a.Utilizador == HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.Current.User.Identity.GetUserId()));

            // Devolver a lista de autorizações se existirem
            if (autorizacoes.Any())
            {
                List<AutorizacaoViewModel> aut = new List<AutorizacaoViewModel>();
                foreach (Autorizacao a in autorizacoes)
                {
                    aut.Add(new AutorizacaoViewModel()
                    {
                        Id = a.Id,
                        Aplicacao = a.Aplicacao.Nome,
                        Validade = a.Validade,
                        Teste = a.Teste
                    });
                }
                return Ok(aut);
            }

            // Devolver NotFound se não existirem autorizações (impossível?)
            return NotFound();
        }

        [Route("Validas")]
        [HttpGet]
        public IHttpActionResult Validas()
        {
            // Carregar as autorizações válidas do utilizador atual
            IQueryable<Autorizacao> autorizacoes = db.Autorizacoes.Where(a => !a.Revogada && a.Validade > DateTime.Now && a.Utilizador == HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.Current.User.Identity.GetUserId()));

            // Devolver a lista de autorizações se existirem
            if (autorizacoes.Any())
            {
                List<AutorizacaoViewModel> aut = new List<AutorizacaoViewModel>();
                foreach (Autorizacao a in autorizacoes)
                {
                    aut.Add(new AutorizacaoViewModel()
                    {
                        Id = a.Id,
                        Aplicacao = a.Aplicacao.Nome,
                        Validade = a.Validade,
                        Teste = a.Teste
                    });
                }
                return Ok(aut);
            }

            // Devolver NotFound se não existirem autorizações
            return NotFound();
        }

        [Route("Teste")]
        [HttpGet]
        public IHttpActionResult Teste()
        {
            // Carregar a autorização do utilizador atual
            IQueryable<Autorizacao> autorizacoes = db.Autorizacoes.Where(a => a.Teste && !a.Revogada && a.Validade > DateTime.Now && a.Utilizador == HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.Current.User.Identity.GetUserId()));

            // Devolver a lista de autorizações se existirem
            if (autorizacoes.Any())
            {
                Autorizacao a = autorizacoes.FirstOrDefault();
                return Ok(new AutorizacaoViewModel()
                {
                    Id = a.Id,
                    Aplicacao = a.Aplicacao.Nome,
                    Validade = a.Validade,
                    Teste = a.Teste
                });
            }

            // Devolver NotFound se não existirem autorizações
            return NotFound();
        }
    }
}