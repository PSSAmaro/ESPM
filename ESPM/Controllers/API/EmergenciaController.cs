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
using System.Web.Http.Description;
using ESPM.Filters;

namespace ESPM.Controllers.API
{
    // Convém depois melhorar a documentação...
    /// <summary>
    /// Criar e atualizar pedidos reais.
    /// </summary>
    // Ver qual é a convenção com a questão da segurança e os hashes, etc...
    // A rota é definida aqui para separar da rota da API de gestão
    [Permissao]
    [Validacao(Teste = false)]
    [Route("api/Emergencia/{id:guid?}")]
    public class EmergenciaController : ApiController
    {
        private GestorPedidos db = new GestorPedidos();

        /// <summary>
        /// Ver o estado atual de um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        /// <returns>Estado atual do pedido e hora da última modificação.</returns>
        [ResponseType(typeof(EstadoAtualViewModel))]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            // Escolher o pedido com o id
            Pedido pedido = await db.Pedido(id);

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
        // Talvez aceitar outras formas de timestamps
        [ResponseType(typeof(RecebidoViewModel))]
        public async Task<IHttpActionResult> Post(EmergenciaViewModel emergencia)
        {
            if (ModelState.IsValid)
            {
                // Escolher a autorização válida
                Autorizacao autorizacao = db.Autorizacao(emergencia.Aplicacao);

                ValidacaoV validacao = new ValidacaoV(emergencia, autorizacao, Request.Headers);

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
        /// <param name="atualizacao">Novas informações.</param>
        public async Task<IHttpActionResult> Put(Guid id, AtualizacaoViewModel atualizacao)
        {
            // Adiciona uma nova localização ao pedido

            // Escolher o pedido com o id
            Pedido pedido = await db.Pedido(id);

            // Devolver NotFound se não existir
            if (pedido == null)
                return NotFound();

            ValidacaoV validacao = new ValidacaoV(atualizacao, pedido, Request.Headers);

            if (validacao.Resultado == Resultado.Valido)
            {
                await db.AtualizarPedido(pedido, atualizacao);
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
            Pedido pedido = await db.Pedido(id);

            // Devolver NotFound se não existir
            if (pedido == null)
                return NotFound();

            ValidacaoV validacao = new ValidacaoV(pedido, Request.Headers);

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
