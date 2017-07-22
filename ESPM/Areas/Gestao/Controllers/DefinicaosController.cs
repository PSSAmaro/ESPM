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

namespace ESPM.Areas.Gestao.Controllers
{
    public class DefinicaosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Gestao/Definicaos
        public async Task<ActionResult> Index()
        {
            return View(await db.Definicoes.ToListAsync());
        }

        // GET: Gestao/Definicaos/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Definicao definicao = await db.Definicoes.FindAsync(id);
            if (definicao == null)
            {
                return HttpNotFound();
            }
            return View(definicao);
        }

        // GET: Gestao/Definicaos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gestao/Definicaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Nome,Maximo,Valor,Apresentacao,Descricao,SignificadoZero,SignificadoOutro,Listar")] Definicao definicao)
        {
            if (ModelState.IsValid)
            {
                db.Definicoes.Add(definicao);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(definicao);
        }

        // GET: Gestao/Definicaos/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Definicao definicao = await db.Definicoes.FindAsync(id);
            if (definicao == null)
            {
                return HttpNotFound();
            }
            return View(definicao);
        }

        // POST: Gestao/Definicaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Nome,Maximo,Valor,Apresentacao,Descricao,SignificadoZero,SignificadoOutro,Listar")] Definicao definicao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(definicao).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(definicao);
        }

        // GET: Gestao/Definicaos/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Definicao definicao = await db.Definicoes.FindAsync(id);
            if (definicao == null)
            {
                return HttpNotFound();
            }
            return View(definicao);
        }

        // POST: Gestao/Definicaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Definicao definicao = await db.Definicoes.FindAsync(id);
            db.Definicoes.Remove(definicao);
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
