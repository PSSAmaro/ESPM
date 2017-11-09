using ESPM.Models;
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
    /// <summary>
    /// Criar e atualizar pedidos reais.
    /// </summary>
    // A rota é definida aqui para separar da rota da API de gestão
    [Permissao]
    [Validacao(Teste = false)]
    [RoutePrefix("api/Emergencia/{id:guid?}")]
    public class EmergenciaController : ApiController
    {
        /// <summary>
        /// Contexto da BD.
        /// </summary>
        protected ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Ver o estado atual de um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        /// <returns>Estado atual do pedido e hora da última modificação.</returns>
        [Route]
        [ResponseType(typeof(EstadoAtualViewModel))]
        public virtual async Task<IHttpActionResult> Get(Guid id)
        {
            Pedido pedido = await db.Pedidos.FindAsync(id);

            return Ok(new EstadoAtualViewModel()
            {
                Estado = pedido.EstadoAtual.Nome,
                Modificado = pedido.EstadoModificado
            });
        }

        /// <summary>
        /// Enviar um novo pedido de ajuda.
        /// </summary>
        /// <param name="emergencia">Dados do pedido de ajuda.</param>
        [Route]
        [ResponseType(typeof(RecebidoViewModel))]
        public virtual async Task<IHttpActionResult> Post(EmergenciaViewModel emergencia)
        {
            AvaliacaoPedido avaliacao = db.Avaliacoes.Find((int)Request.Properties["Avaliacao"]);

            Pedido pedido = new Pedido(emergencia, await db.Estados.FindAsync(db.Definicoes.Find("EstadoInicial").Valor), avaliacao, await db.Autorizacoes.FindAsync(avaliacao.Header));

            db.Pedidos.Add(pedido);

            await db.SaveChangesAsync();

            return Ok(new RecebidoViewModel()
            {
                Recebido = DateTime.Now,
                Id = pedido.Id
            });
        }

        /// <summary>
        /// Enviar uma nova localização para um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        /// <param name="emergencia">Novas informações.</param>
        [Route]
        [ResponseType(typeof(AtualizadoViewModel))]
        public virtual async Task<IHttpActionResult> Put(Guid id, EmergenciaViewModel emergencia)
        {
            Pedido pedido = await db.Pedidos.FindAsync(id);

            DateTime modificado = pedido.Modificado;

            AvaliacaoPedido avaliacao = db.Avaliacoes.Find((int)Request.Properties["Avaliacao"]);

            pedido.SetNome(emergencia.Nome);
            pedido.SetContacto(emergencia.Contacto);
            pedido.SetIdade(emergencia.Idade);
            pedido.SetOD(emergencia.OutrosDetalhesPessoa);
            pedido.SetDescricao(emergencia.Descricao);
            foreach (LocalizacaoViewModel localizacao in emergencia.Localizacoes)
                pedido.SetLocalizacao(localizacao, avaliacao);
            foreach (ImagemViewModel imagem in emergencia.Imagens)
                pedido.SetImagem(imagem, avaliacao);

            await db.SaveChangesAsync();

            return Ok(new AtualizadoViewModel()
            {
                Recebido = DateTime.Now,
                Atualizado = pedido.Modificado != modificado
            });
        }

        /// <summary>
        /// Cancelar um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        [Route]
        [ResponseType(typeof(CanceladoViewModel))]
        public virtual async Task<IHttpActionResult> Delete(Guid id)
        {
            // Não elimina o pedido, apenas altera o seu estado para Anulada

            Pedido pedido = await db.Pedidos.FindAsync(id);

            AvaliacaoPedido avaliacao = db.Avaliacoes.Find((int)Request.Properties["Avaliacao"]);

            pedido.SetEstado(await db.Estados.FindAsync(db.Definicoes.Find("EstadoCancelado").Valor));

            await db.SaveChangesAsync();

            return Ok(new CanceladoViewModel()
            {
                Recebido = DateTime.Now
            });
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
