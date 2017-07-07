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
    /// <summary>
    /// Criar e atualizar pedidos reais.
    /// </summary>
    // Ver qual é a convenção com a questão da segurança e os hashes, etc...
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
            // Escolher o pedido com o id
            Pedido pedido = await db.Pedidos.FindAsync(id);

            // Estado atual
            EstadoDePedido atual = pedido.Estados.OrderByDescending(e => e.Tempo).FirstOrDefault();

            // Devolver o estado atual e a última modificação do estado se existir
            return Ok(new EstadoAtualViewModel()
            {
                Estado = atual.Estado.Nome,
                Modificado = atual.Tempo
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
            // Carregar avaliação das propriedades do pedido
            Avaliacao avaliacao = db.Avaliacoes.Find((int)Request.Properties["Avaliacao"]);

            // Tempos do pedido: Envio (Se existe) e Receção
            DateTime recebido = DateTime.Now;
            DateTime enviado = emergencia.Tempo ?? recebido;

            // Criar o novo pedido
            Pedido pedido = new Pedido()
            {
                Autorizacao = db.Autorizacoes.Find(avaliacao.Header),
                Avaliacao = avaliacao,
                Tempo = enviado,
                Modificado = recebido
            };

            // Adicionar detalhes da pessoa se foram enviados
            if (emergencia.Nome != null || emergencia.Contacto != null || emergencia.Idade != null || emergencia.OutrosDetalhesPessoa != null)
            {
                pedido.InformacaoPessoa.Add(new Pessoa()
                {
                    Avaliacao = avaliacao,
                    Tempo = enviado,
                    Nome = emergencia.Nome,
                    Contacto = emergencia.Contacto,
                    Idade = emergencia.Idade,
                    OutrosDetalhes = emergencia.OutrosDetalhesPessoa
                });
            }

            // Adicionar descrição se foi enviada
            if (emergencia.Descricao != null)
            {
                pedido.Descricoes.Add(new Descricao()
                {
                    Avaliacao = avaliacao,
                    Tempo = enviado,
                    Texto = emergencia.Descricao
                });
            }

            // Adicionar localizações se foram enviadas
            if (emergencia.Localizacoes.Count > 0)
            {
                foreach (LocalizacaoViewModel localizacao in emergencia.Localizacoes)
                {
                    pedido.Localizacoes.Add(new Localizacao()
                    {
                        Avaliacao = avaliacao,
                        Tempo = localizacao.Tempo ?? enviado,
                        Latitude = localizacao.Latitude,
                        Longitude = localizacao.Longitude
                    });
                }
            }

            // Adicionar estado inicial
            pedido.Estados.Add(new EstadoDePedido()
            {
                Avaliacao = avaliacao,
                Estado = db.Estados.Find(db.Definicoes.Find("EstadoInicial").Valor),
                Tempo = recebido
            });

            // Adicionar pedido à BD
            db.Pedidos.Add(pedido);
            await db.SaveChangesAsync();

            return Ok(new RecebidoViewModel()
            {
                Recebido = recebido,
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
            // FALTA: Retirar código repetido

            // Escolher o pedido com o id
            Pedido pedido = await db.Pedidos.FindAsync(id);

            // Carregar avaliação das propriedades do pedido
            Avaliacao avaliacao = db.Avaliacoes.Find((int)Request.Properties["Avaliacao"]);

            // Tempos do pedido: Envio (Se existe) e Receção
            DateTime recebido = DateTime.Now;
            DateTime enviado = emergencia.Tempo ?? recebido;

            // Apesar de todas as verificações, neste momento não é possível garantir que este Put traz novas informações
            // Por isso é necessário utilizar esta variável
            bool alterado = false;

            // Adicionar detalhes da pessoa se foram enviados
            if (emergencia.Nome != null || emergencia.Contacto != null || emergencia.Idade != null || emergencia.OutrosDetalhesPessoa != null)
            {
                // Confirmar que já existe informação antes, se não existir simplesmente criar uma nova pessoa
                if (pedido.InformacaoPessoa.Count == 0)
                {
                    pedido.InformacaoPessoa.Add(new Pessoa()
                    {
                        Avaliacao = avaliacao,
                        Tempo = enviado,
                        Nome = emergencia.Nome,
                        Contacto = emergencia.Contacto,
                        Idade = emergencia.Idade,
                        OutrosDetalhes = emergencia.OutrosDetalhesPessoa
                    });
                    alterado = true;
                }
                else
                {
                    // Carregar as informações já existentes
                    Pessoa pessoa = pedido.InformacaoPessoa.OrderByDescending(d => d.Tempo).FirstOrDefault();

                    // Verificar que alguma informação é realmente diferente
                    if (emergencia.Nome != pessoa.Nome || emergencia.Contacto != pessoa.Contacto || emergencia.Idade != pessoa.Idade || emergencia.OutrosDetalhesPessoa != pessoa.OutrosDetalhes)
                    {
                        // Atualizar os campos que foram enviados e popular os outros com os valores anteriores
                        pedido.InformacaoPessoa.Add(new Pessoa()
                        {
                            Avaliacao = avaliacao,
                            Tempo = enviado,
                            Nome = emergencia.Nome ?? pessoa.Nome,
                            Contacto = emergencia.Contacto ?? pessoa.Contacto,
                            Idade = emergencia.Idade ?? pessoa.Idade,
                            OutrosDetalhes = emergencia.OutrosDetalhesPessoa ?? pessoa.OutrosDetalhes
                        });
                        alterado = true;
                    }
                }
            }

            // Adicionar descrição se foi enviada
            if (emergencia.Descricao != null)
            {
                pedido.Descricoes.Add(new Descricao()
                {
                    Avaliacao = avaliacao,
                    Tempo = enviado,
                    Texto = emergencia.Descricao
                });
                alterado = true;
            }

            // Adicionar localizações se foram enviadas
            if (emergencia.Localizacoes.Count > 0)
            {
                foreach (LocalizacaoViewModel localizacao in emergencia.Localizacoes)
                {
                    pedido.Localizacoes.Add(new Localizacao()
                    {
                        Avaliacao = avaliacao,
                        Tempo = localizacao.Tempo ?? enviado,
                        Latitude = localizacao.Latitude,
                        Longitude = localizacao.Longitude
                    });
                }
                alterado = true;
            }
            
            if (alterado)
            {
                pedido.Modificado = enviado;

                // Guardar alterações
                await db.SaveChangesAsync();
            }

            return Ok(new AtualizadoViewModel()
            {
                Recebido = recebido,
                Atualizado = alterado
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
            // Na verdade não elimina o pedido, apenas altera o seu estado para Anulada

            // Escolher o pedido com o id
            Pedido pedido = await db.Pedidos.FindAsync(id);

            // Falta: Confirmar que veio do mesmo IP e autorização
            // Usar o ActionFilter

            // Momento em que o pedido de cancelamento foi recebido
            DateTime tempo = DateTime.Now;

            // Adicionar o estado correspondente ao pedido anulado
            pedido.Estados.Add(new EstadoDePedido()
            {
                Estado = db.Estados.Find(db.Definicoes.Find("EstadoCancelado").Valor),
                Tempo = tempo
            });

            return Ok(new CanceladoViewModel()
            {
                Recebido = tempo
            });
        }

        /// <summary>
        /// Enviar imagens referentes a um pedido de ajuda.
        /// </summary>
        /// <param name="id">ID do pedido de ajuda.</param>
        [Route("Imagem")]
        public virtual async Task<IHttpActionResult> Imagem(Guid id)
        {
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
    }
}
