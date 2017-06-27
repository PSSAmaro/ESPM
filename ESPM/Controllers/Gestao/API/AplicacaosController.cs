using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ESPM.Models;

namespace ESPM.Controllers.Gestao.API
{
    // FICHEIRO A USAR COMO TEMPLATE PARA DEPOIS ELIMINAR
    public class AplicacaosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Aplicacaos
        public IQueryable<Aplicacao> GetAplicacoes()
        {
            return db.Aplicacoes;
        }

        // GET: api/Aplicacaos/5
        [ResponseType(typeof(Aplicacao))]
        public async Task<IHttpActionResult> GetAplicacao(Guid id)
        {
            Aplicacao aplicacao = await db.Aplicacoes.FindAsync(id);
            if (aplicacao == null)
            {
                return NotFound();
            }

            return Ok(aplicacao);
        }

        // PUT: api/Aplicacaos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAplicacao(Guid id, Aplicacao aplicacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aplicacao.Id)
            {
                return BadRequest();
            }

            db.Entry(aplicacao).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AplicacaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Aplicacaos
        [ResponseType(typeof(Aplicacao))]
        public async Task<IHttpActionResult> PostAplicacao(Aplicacao aplicacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Aplicacoes.Add(aplicacao);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = aplicacao.Id }, aplicacao);
        }

        // DELETE: api/Aplicacaos/5
        [ResponseType(typeof(Aplicacao))]
        public async Task<IHttpActionResult> DeleteAplicacao(Guid id)
        {
            Aplicacao aplicacao = await db.Aplicacoes.FindAsync(id);
            if (aplicacao == null)
            {
                return NotFound();
            }

            db.Aplicacoes.Remove(aplicacao);
            await db.SaveChangesAsync();

            return Ok(aplicacao);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AplicacaoExists(Guid id)
        {
            return db.Aplicacoes.Count(e => e.Id == id) > 0;
        }
    }
}