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

namespace ESPM.Controllers.API
{
    // Convém depois melhorar a documentação...
    /// <summary>
    /// Criar e atualizar pedidos reais.
    /// </summary>
    // Ver qual é a convenção com a questão da segurança e os hashes, etc...
    public class EmergenciaController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Ver o estado atual de um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        public async Task<IHttpActionResult> Get(Guid id)
        {
            // Escolher o pedido com o id
            Pedido pedido = await db.Pedidos.FindAsync(id);

            // Devolver NotFound se não existir
            if (pedido == null)
                return NotFound();

            // Devolver o estado atual e a última modificação do estado se existir
            return Ok(new EstadoAtualViewModel() {
                Estado = pedido.EstadoAtual().Estado.Nome,
                Modificado = pedido.EstadoAtual().Tempo
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
                // Momento em que o pedido foi recebido
                DateTime recebido = DateTime.Now;

                // Escolher a autorização válida
                Autorizacao aut = db.Autorizacoes.Where(a => a.Aplicacao.Id == emergencia.Aplicacao && !a.Teste && !a.Revogada && a.Validade > recebido).FirstOrDefault();

                // Se não foi encontrada nenhuma autorização válida para a aplicação usada ou o header hash é inexistente/inválido
                if (aut == null || !Request.Headers.Contains("Hash") || Request.Headers.GetValues("Hash").First() != Hash(emergencia.ToString(), aut.Id))
                    return BadRequest("Erro de autenticação");

                // Apesar do modelo válido convém confirmar que foram enviadas informações suficientes
                // Se todos os seguintes campos forem nulos, é considerado que não há informações suficientes
                if (emergencia.Contacto == null && emergencia.OutrosDetalhesPessoa == null && emergencia.Descricao == null && !(emergencia.Latitude != null && emergencia.Longitude != null))
                    return BadRequest("Informações insuficientes");

                // FALTA: Definir a credibilidade do pedido

                // Criar o novo pedido
                Pedido pedido = new Pedido()
                {
                    // Associar a autorização usada
                    Autorizacao = aut,
                    // Usar o tempo recebido ou o atual
                    Tempo = (emergencia.Tempo == null ? recebido : (DateTime)emergencia.Tempo),
                    Nome = emergencia.Nome,
                    Contacto = emergencia.Contacto,
                    Idade = emergencia.Idade,
                    OutrosDetalhesPessoa = emergencia.OutrosDetalhesPessoa
                };

                // Adicionar descrição se existir descrição
                if(emergencia.Descricao != null)
                {
                    pedido.Descricoes = new List<Descricao>
                    {
                        new Descricao()
                        {
                            Tempo = pedido.Tempo,
                            Texto = emergencia.Descricao
                        }
                    };
                }

                // Adicionar localização se existir localização
                if (emergencia.Latitude != null && emergencia.Longitude != null)
                {
                    pedido.Localizacoes = new List<Localizacao>
                    {
                        new Localizacao()
                        {
                            Tempo = pedido.Tempo,
                            Latitude = emergencia.Latitude,
                            Longitude = emergencia.Longitude
                        }
                    };
                }

                // Criar o estado inicial
                pedido.Estados = new List<EstadoDePedido>
                {
                    new EstadoDePedido()
                    {
                        // Verificar se dá para usar a função EstadoInicial()
                        Estado = db.Estados.Where(e => e.Anteriores.Count == 0).FirstOrDefault(),
                        Tempo = recebido
                    }
                };

                // Adicionar o novo pedido à BD
                db.Pedidos.Add(pedido);
                await db.SaveChangesAsync();

                return Ok(new RecebidoViewModel() {
                    Id = pedido.Id
                });
            }
            return BadRequest("Formato inválido");
        }

        /// <summary>
        /// Enviar uma nova localização para um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        /// <param name="localizacao">Nova localização.</param>
        [HttpPost]
        [Route("api/emergencia/{id}/localizacao")]
        public async Task<IHttpActionResult> Localizacao(Guid id, LocalizacaoViewModel localizacao)
        {
            // Adiciona uma nova localização ao pedido

            // Escolher o pedido com o id
            Pedido pedido = await db.Pedidos.FindAsync(id);

            // Devolver NotFound se não existir
            if (pedido == null)
                return NotFound();

            db.Localizacoes.Add(new Localizacao()
            {
                Pedido = pedido,
                // Usar o tempo recebido ou o atual
                Tempo = (localizacao.Tempo == null ? DateTime.Now : (DateTime)localizacao.Tempo),
                Latitude = localizacao.Latitude,
                Longitude = localizacao.Longitude
            });

            await db.SaveChangesAsync();

            // Devolve um Ok vazio
            return Ok();
        }

        /// <summary>
        /// Cancelar um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            // Na verdade não elimina o pedido, apenas altera o seu estado para Anulada

            // Escolher o pedido com o id
            Pedido pedido = await db.Pedidos.FindAsync(id);

            // Devolver NotFound se não existir
            if (pedido == null)
                return NotFound();

            // Hash do Guid do pedido + Guid da autorização
            if (!Request.Headers.Contains("Hash") || Request.Headers.GetValues("Hash").First() != Hash(id.ToString(), pedido.Autorizacao.Id))
                return BadRequest("Erro de autenticação");

            // Adicionar o estado Anulada
            db.EstadosDePedido.Add(new EstadoDePedido()
            {
                Pedido = pedido,
                Estado = db.Estados.Where(e => e.Nome == "Anulada").FirstOrDefault(),
                Tempo = DateTime.Now
            });

            await db.SaveChangesAsync();

            // Devolve um Ok vazio
            return Ok();
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

        private string Hash(string s, Guid g)
        {
            string str = s + g;
            // https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
            byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(str));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }
    }
}
