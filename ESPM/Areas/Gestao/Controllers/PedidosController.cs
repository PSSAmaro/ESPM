using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ESPM.Models;
using ESPM.Areas.Gestao.Models;
using System.Threading;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ESPM.Areas.Gestao.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PedidosController : Controller
    {
        private ApplicationDbContext db = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();

        public async Task<ActionResult> Index()
        {
            List<ResumoPedidoViewModel> pedidos = new List<ResumoPedidoViewModel>();
            // Filtrar para os abertos
            foreach (Pedido p in db.Pedidos.ToList().Where(pd => !pd.EstadoAtual.Final))
                pedidos.Add(new ResumoPedidoViewModel(p));

            return View(pedidos);
        }

        public async Task<ActionResult> Editar(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Pedido pedido = await db.Pedidos.FindAsync(id);
            if (pedido == null)
                return HttpNotFound();

            return View(new PedidoViewModel(pedido));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(PedidoEditadoViewModel pedido)
        {
            if (ModelState.IsValid)
            {
                Pedido p = await db.Pedidos.FindAsync(pedido.Id);
                ApplicationUser u = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                p.SetEstado(await db.Estados.FindAsync(pedido.Estado), u);
                p.SetNome(pedido.Nome, u);
                p.SetContacto(pedido.Contacto, u);
                p.SetIdade(pedido.Idade, u);
                p.SetOD(pedido.OutrosDetalhes, u);
                p.SetDescricao(pedido.Descricao, u);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pedido);
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
