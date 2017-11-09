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
using System.Web.Http.Description;

namespace ESPM.Areas.Gestao.Controllers.API
{
    [Authorize(Roles = "Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
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
                estados.Add(new ResumoEstadoViewModel(e));
            }

            return Ok(new ListaEstadosViewModel()
            {
                Estados = estados,
                Inicial = inicial.Valor,
                Cancelado = cancelado.Valor
            });
        }

        [HttpPost]
        public async Task<IHttpActionResult> Novo(NovoEstadoViewModel estado)
        {
            if (ModelState.IsValid)
            {
                Estado e = new Estado()
                {
                    Nome = estado.Nome,
                    Familia = 0,
                    Ativo = false
                };
                db.Estados.Add(e);
                await db.SaveChangesAsync();
                return Ok(new ResumoEstadoViewModel(e));
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Editar(EditarEstadosViewModel estados)
        {
            if (ModelState.IsValid)
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
            return BadRequest();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
