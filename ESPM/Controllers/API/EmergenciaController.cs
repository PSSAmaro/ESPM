using ESPM.Models;
using ESPM.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ESPM.Controllers.API
{
    // Convém depois melhorar a documentação...
    /// <summary>
    /// Criar e atualizar pedidos reais.
    /// </summary>
    // Ver qual é a convenção com a questão da segurança e os hashes, etc...
    public class EmergenciaController : ApiController
    {
        private GestorPedidos db = new GestorPedidos();

        /// <summary>
        /// Ver o estado atual de um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        public async Task<IHttpActionResult> Get(Guid id)
        {
            // Escolher o pedido com o id
            Pedido pedido = await db.LerPedido(id);

            // Devolver NotFound se não existir
            if (pedido == null)
                return NotFound();

            // Devolver o estado atual e a última modificação do estado se existir
            EstadoDePedido atual = pedido.Estados.OrderByDescending(e => e.Tempo).FirstOrDefault();
            return Ok(new EstadoAtualViewModel() {
                Estado = atual.Estado.Nome,
                Modificado = atual.Tempo
            });
        }

        /// <summary>
        /// Enviar um novo pedido de ajuda.
        /// </summary>
        /// <param name="emergencia">Dados do pedido de ajuda.</param>
        // Novo pedido
        // Faltam imagens e várias localizações
        // Talvez aceitar outras formas de timestamps
        public async Task<IHttpActionResult> Post(EmergenciaViewModel emergencia)
        {
            if (ModelState.IsValid)
            {
                // Escolher a autorização válida
                Autorizacao autorizacao = db.LerAutorizacao(emergencia.Aplicacao);

                Validacao validacao = new Validacao(emergencia, autorizacao, Request.Headers.GetValues("Hash"));

                // Era bom não responder BadRequest a tudo...
                // E falta guardar os pedidos com erro :/
                if(validacao.Resultado == Resultado.Valido)
                {
                    Pedido pedido = await db.CriarPedido(emergencia, autorizacao);
                    return Ok(new RecebidoViewModel()
                    {
                        Id = pedido.Id
                    });
                }
                else
                    return BadRequest(validacao.Mensagem[(int)validacao.Resultado]);
            }
            return BadRequest("Formato inválido");
        }

        /// <summary>
        /// Enviar uma nova localização para um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        /// <param name="localizacoes">Nova localização.</param>
        [HttpPost]
        [Route("api/emergencia/{id}/localizacao")]
        public async Task<IHttpActionResult> Localizacao(Guid id, List<LocalizacaoViewModel> localizacoes)
        {
            // Adiciona uma nova localização ao pedido

            // Escolher o pedido com o id
            Pedido pedido = await db.LerPedido(id);

            // Devolver NotFound se não existir
            if (pedido == null)
                return NotFound();

            Validacao validacao = new Validacao(localizacoes, pedido.Autorizacao, Request.Headers.GetValues("Hash"));

            if (validacao.Resultado == Resultado.Valido)
            {
                await db.CriarLocalizacoes(localizacoes, pedido);
                return Ok();
            }
            else
                return BadRequest(validacao.Mensagem[(int)validacao.Resultado]);
        }

        /// <summary>
        /// Cancelar um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            // Na verdade não elimina o pedido, apenas altera o seu estado para Anulada

            // Escolher o pedido com o id
            Pedido pedido = await db.LerPedido(id);

            // Devolver NotFound se não existir
            if (pedido == null)
                return NotFound();

            Validacao validacao = new Validacao(pedido, Request.Headers.GetValues("Hash"));

            if (validacao.Resultado == Resultado.Valido)
            {
                await db.CancelarPedido(pedido);
                return Ok();
            }
            else
                return BadRequest(validacao.Mensagem[(int)validacao.Resultado]);
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
