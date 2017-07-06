using ESPM.Filters;
using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ESPM.Controllers.API
{
    /// <summary>
    /// Criar e atualizar pedidos de teste.
    /// </summary>
    [Permissao]
    [Validacao(Teste = true)]
    [RoutePrefix("api/Teste/{id:guid?}")]
    public class TesteController : EmergenciaController
    {
        /// <summary>
        /// Ver o estado atual de um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        /// <returns>Estado atual do pedido e hora da última modificação.</returns>
        [Route]
        [ResponseType(typeof(EstadoAtualViewModel))]
        public override async Task<IHttpActionResult> Get(Guid id)
        {
            return await base.Get(id);
        }

        /// <summary>
        /// Enviar um novo pedido de ajuda.
        /// </summary>
        /// <param name="emergencia">Dados do pedido de ajuda.</param>
        [Route]
        [ResponseType(typeof(RecebidoViewModel))]
        public override async Task<IHttpActionResult> Post(EmergenciaViewModel emergencia)
        {
            return await base.Post(emergencia);
        }

        /// <summary>
        /// Enviar uma nova localização para um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        /// <param name="atualizacao">Novas informações.</param>
        [Route]
        public override async Task<IHttpActionResult> Put(Guid id, AtualizacaoViewModel atualizacao)
        {
            return await base.Put(id, atualizacao);
        }

        /// <summary>
        /// Cancelar um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        [Route]
        public override async Task<IHttpActionResult> Delete(Guid id)
        {
            return await base.Delete(id);
        }

        /// <summary>
        /// Enviar imagens referentes a um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        [Route("Imagem")]
        public override async Task<IHttpActionResult> Imagem(Guid id)
        {
            return await base.Imagem(id);
        }

        /// <summary>
        /// Libertar recursos.
        /// </summary>
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