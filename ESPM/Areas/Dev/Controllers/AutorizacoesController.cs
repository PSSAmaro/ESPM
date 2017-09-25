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

namespace ESPM.Areas.Dev.Controllers
{
    public class AutorizacoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dev/Autorizacoes
        public async Task<ActionResult> Index()
        {
            return View(await db.Autorizacoes.ToListAsync());
        }

        // GET: Dev/Autorizacoes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autorizacao autorizacao = await db.Autorizacoes.FindAsync(id);
            if (autorizacao == null)
            {
                return HttpNotFound();
            }
            return View(autorizacao);
        }

        // GET: Dev/Autorizacoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dev/Autorizacoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Validade,Teste,Revogada")] Autorizacao autorizacao)
        {
            if (ModelState.IsValid)
            {
                autorizacao.Id = Guid.NewGuid();
                db.Autorizacoes.Add(autorizacao);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(autorizacao);
        }

        // GET: Dev/Autorizacoes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autorizacao autorizacao = await db.Autorizacoes.FindAsync(id);
            if (autorizacao == null)
            {
                return HttpNotFound();
            }
            return View(autorizacao);
        }

        // POST: Dev/Autorizacoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Validade,Teste,Revogada")] Autorizacao autorizacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(autorizacao).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(autorizacao);
        }

        // GET: Dev/Autorizacoes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autorizacao autorizacao = await db.Autorizacoes.FindAsync(id);
            if (autorizacao == null)
            {
                return HttpNotFound();
            }
            return View(autorizacao);
        }

        // POST: Dev/Autorizacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Autorizacao autorizacao = await db.Autorizacoes.FindAsync(id);
            db.Autorizacoes.Remove(autorizacao);
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
