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

namespace ESPM.Areas.Gestao.Controllers
{
    public class EstadosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Gestao/Estados
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: Gestao/Estados/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = await db.Estados.FindAsync(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // GET: Gestao/Estados/Create
        public ActionResult Criar()
        {
            return View();
        }

        // POST: Gestao/Estados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar([Bind(Include = "Id,Nome,Familia,Ativo")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Estados.Add(estado);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(estado);
        }

        // GET: Gestao/Estados/Edit/5
        public async Task<ActionResult> Editar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Estado e = await db.Estados.FindAsync(id);
            if (e == null)
                return HttpNotFound();

            Definicao inicial = await db.Definicoes.FindAsync("EstadoInicial");
            Definicao cancelado = await db.Definicoes.FindAsync("EstadoCancelado");
            Definicao familias = await db.Definicoes.FindAsync("Familias");

            EstadoViewModel estado = new EstadoViewModel()
            {
                Id = e.Id,
                Nome = e.Nome,
                Ativo = e.Ativo,
                Icone = e.Familia,
                Inicial = inicial.Valor == e.Id,
                Cancelado = cancelado.Valor == e.Id,
                Familias = familias.Valor
            };

            foreach (TransicaoDeEstado t in e.Anteriores)
            {
                estado.Anteriores.Add(new ResumoEstadoViewModel()
                {
                    Id = t.De.Id,
                    Nome = t.De.Nome,
                    Ativo = t.De.Ativo,
                    Icone = "/Content/Imagens/Estados/" + t.De.Familia + ".png"
                });
            }

            foreach (TransicaoDeEstado t in e.Seguintes)
            {
                estado.Seguintes.Add(new ResumoEstadoViewModel()
                {
                    Id = t.Para.Id,
                    Nome = t.Para.Nome,
                    Ativo = t.Para.Ativo,
                    Icone = "/Content/Imagens/Estados/" + t.Para.Familia + ".png"
                });
            }

            return View(estado);
        }

        // POST: Gestao/Estados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar([Bind(Include = "Id,Nome,Familia,Ativo")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estado).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(estado);
        }

        // GET: Gestao/Estados/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = await db.Estados.FindAsync(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // POST: Gestao/Estados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Estado estado = await db.Estados.FindAsync(id);
            db.Estados.Remove(estado);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
