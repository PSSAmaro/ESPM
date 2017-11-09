using ESPM.Areas.Gestao.Models;
using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ESPM.Areas.Gestao.Controllers.API
{
    [Authorize(Roles = "Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PedidosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public async Task<IHttpActionResult> Index()
        {
            List<ResumoPedidoViewModel> pedidos = new List<ResumoPedidoViewModel>();
            
            foreach (Pedido p in db.Pedidos.ToList().Where(pd => !pd.EstadoAtual.Final))
                pedidos.Add(new ResumoPedidoViewModel(p));

            return Ok(pedidos);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Fechados()
        {
            List<ResumoPedidoViewModel> pedidos = new List<ResumoPedidoViewModel>();

            foreach (Pedido p in db.Pedidos.ToList().Where(pd => pd.EstadoAtual.Final))
                pedidos.Add(new ResumoPedidoViewModel(p));

            return Ok(pedidos);
        }
    }
}