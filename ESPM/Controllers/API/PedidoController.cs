using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ESPM.Controllers.API
{
    // Usar para o módulo de gestão
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "Operador")]
    public class PedidoController : ApiController
    {
        private ApplicationDbContext db;
    }
}
