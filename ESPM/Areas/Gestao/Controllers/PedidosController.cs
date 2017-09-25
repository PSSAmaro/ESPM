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
    public class PedidosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Gestao/Pedidos
        public async Task<ActionResult> Index()
        {
            List<ResumoPedidoViewModel> pedidos = new List<ResumoPedidoViewModel>();
            // Filtrar para os abertos
            foreach (Pedido p in db.Pedidos.ToList())
            {
                Estado estado = p.Estados.OrderByDescending(e => e.Tempo).FirstOrDefault().Estado;
                Localizacao localizacao = p.Localizacoes.OrderByDescending(l => l.Tempo).FirstOrDefault();
                Pessoa pessoa = p.InformacaoPessoa.OrderByDescending(i => i.Tempo).FirstOrDefault();
                ResumoEstadoViewModel re = new ResumoEstadoViewModel()
                {
                    Id = estado.Id,
                    Nome = estado.Nome,
                    Ativo = estado.Ativo,
                    Icone = "/Content/Imagens/Estados/" + estado.Familia + ".png"
                };
                pedidos.Add(new ResumoPedidoViewModel()
                {
                    Id = p.Id,
                    Estado = re,
                    Recebido = p.Tempo,
                    Modificado = p.Modificado,
                    Latitude = localizacao.Latitude,
                    Longitude = localizacao.Longitude,
                    Nome = pessoa.Nome,
                    Contacto = pessoa.Contacto
                });
            }
            return View(pedidos);
        }

        // GET: Gestao/Pedidos/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = await db.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ResumoPedidoViewModel p = new ResumoPedidoViewModel()
            {
                Nome = pedido.InformacaoPessoa.OrderByDescending(i => i.Tempo).FirstOrDefault().Nome,
                Contacto = pedido.InformacaoPessoa.OrderByDescending(i => i.Tempo).FirstOrDefault().Contacto
            };
            return View(p);
        }

        // GET: Gestao/Pedidos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gestao/Pedidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Tempo,Modificado")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                pedido.Id = Guid.NewGuid();
                db.Pedidos.Add(pedido);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        // GET: Gestao/Pedidos/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = await db.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            Estado estado = pedido.Estados.OrderByDescending(e => e.Tempo).FirstOrDefault().Estado;
            Localizacao localizacao = pedido.Localizacoes.OrderByDescending(l => l.Tempo).FirstOrDefault();
            Pessoa pessoa = pedido.InformacaoPessoa.OrderByDescending(i => i.Tempo).FirstOrDefault();
            // Descricao descricao = pedido.Descricoes.OrderByDescending(d => d.Tempo).FirstOrDefault();
            Descricao descricao = new Descricao();
            ResumoEstadoViewModel re = new ResumoEstadoViewModel()
            {
                Id = estado.Id,
                Nome = estado.Nome,
                Ativo = estado.Ativo,
                Icone = "/Content/Imagens/Estados/" + estado.Familia + ".png"
            };
            return View(new PedidoViewModel()
            {
                Id = pedido.Id,
                Estado = re,
                Latitude = localizacao.Latitude,
                Longitude = localizacao.Longitude,
                Nome = pessoa.Nome,
                Contacto = pessoa.Contacto,
                Descricao = descricao.Texto
            });
        }

        // POST: Gestao/Pedidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Tempo,Modificado")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pedido);
        }

        // GET: Gestao/Pedidos/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = await db.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Gestao/Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Pedido pedido = await db.Pedidos.FindAsync(id);
            db.Pedidos.Remove(pedido);
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
