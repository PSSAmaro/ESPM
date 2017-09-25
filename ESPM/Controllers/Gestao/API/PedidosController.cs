/*using ESPM.Helpers;
using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ESPM.Controllers.Gestao.API
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "Operador")]
    public class PedidosController : ApiController
    {
        private GestorPedidos db = new GestorPedidos();

        [HttpGet]
        public IHttpActionResult Todos()
        {
            return Ok(db.Pedidos().Select(p => new ResumoPedidoViewModel(p)));
        }

        [HttpGet]
        public IHttpActionResult Abertos()
        {
            return Ok(db.PedidosAbertos().Select(p => new ResumoPedidoViewModel(p)));
        }

        [HttpGet]
        public IHttpActionResult Fechados()
        {
            return Ok(db.PedidosFechados().Select(p => new ResumoPedidoViewModel(p)));
        }

        [HttpGet]
        public async Task<IHttpActionResult> Detalhes(Guid id)
        {
            Pedido pedido = await db.Pedido(id);

            if (pedido == null)
                return NotFound();

            return Ok();
        }

        public async Task<IHttpActionResult> Editar(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IHttpActionResult> AlterarEstado(Guid id, AlterarEstadoViewModel estado)
        {
            Pedido pedido = await db.Pedido(id);

            if (pedido == null)
                return NotFound();

            if (db.AlterarEstadoDePedido(estado.Nome, pedido) == null)
                return BadRequest("Transição não permitida");
            return Ok();
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
*/