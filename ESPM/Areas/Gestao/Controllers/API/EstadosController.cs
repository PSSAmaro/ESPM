using ESPM.Areas.Gestao.Models;
using ESPM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ESPM.Areas.Gestao.Controllers.API
{
    public class EstadosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public async Task<IHttpActionResult> Index()
        {
            Definicao inicial = await db.Definicoes.FindAsync("EstadoInicial");
            Definicao cancelado = await db.Definicoes.FindAsync("EstadoCancelado");
            List<ResumoEstadoViewModel> estados = new List<ResumoEstadoViewModel>();
            foreach (Estado e in db.Estados)
            {
                estados.Add(new ResumoEstadoViewModel()
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Ativo = e.Ativo,
                    Imagem = "/Content/Imagens/Estados/" + e.Familia + ".png"
                });
            }

            return Ok(new ListaEstadosViewModel()
            {
                Estados = estados,
                Inicial = inicial.Valor,
                Cancelado = cancelado.Valor
            });
        }

        [HttpPost]
        public async Task<IHttpActionResult> Editar(EditarEstadosViewModel estados)
        {
            // Ativar/Desativar estados
            foreach (Estado e in db.Estados)
                e.Ativo = estados.Ativos.Contains(e.Id);

            // Confirmar que os estados pretendidos existem
            if (db.Estados.Count(e => e.Id == estados.Inicial) > 0 && db.Estados.Count(e => e.Id == estados.Cancelado) > 0)
            {
                // Alterar os estados inicial e cancelado
                Definicao inicial = await db.Definicoes.FindAsync("EstadoInicial");
                Definicao cancelado = await db.Definicoes.FindAsync("EstadoCancelado");
                ApplicationUser utilizador = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.Current.User.Identity.GetUserId());

                if (inicial.Alterar(estados.Inicial, utilizador) == -2 || cancelado.Alterar(estados.Cancelado, utilizador) == -2)
                    return BadRequest();
            }
            else
                return BadRequest();

            // Guardar alterações
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
