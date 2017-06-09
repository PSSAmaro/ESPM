using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ESPM.Helpers
{
    public class GestaoBD
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Devolve o pedido com o id definido.
        /// Devolve null se não for encontrado.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        public async Task<Pedido> GetPedido(Guid id)
        {
            return await db.Pedidos.FindAsync(id);
        }

        /// <summary>
        /// Inserir um novo pedido na base de dados.
        /// </summary>
        /// <param name="emergencia">A emergência recebida.</param>
        /// <param name="autorizacao">A autorização utilizada pela aplicação.</param>
        /// <returns></returns>
        // Falta guardar os pedidos com erro... ups
        public async Task<RecebidoViewModel> PostPedido(EmergenciaViewModel emergencia, Autorizacao autorizacao)
        {
            DateTime t = DateTime.Now;
            Pedido pedido = new Pedido()
            {
                // Associar a autorização usada
                Autorizacao = autorizacao,
                // Usar o tempo recebido ou o atual
                Tempo = (emergencia.Tempo == null ? t : (DateTime)emergencia.Tempo),
                Nome = emergencia.Nome,
                Contacto = emergencia.Contacto,
                Idade = emergencia.Idade,
                OutrosDetalhesPessoa = emergencia.OutrosDetalhesPessoa
            };

            // Adicionar descrição se existir descrição
            if (emergencia.Descricao != null)
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
                        Tempo = t
                    }
                };

            // Adicionar o novo pedido à BD
            db.Pedidos.Add(pedido);
            await db.SaveChangesAsync();

            return new RecebidoViewModel()
            {
                Id = pedido.Id
            };
        }

        /// <summary>
        /// Marca um pedido como anulado.
        /// </summary>
        /// <param name="pedido">Pedido a anular.</param>
        /// <returns></returns>
        public async Task<int> DeletePedido(Pedido pedido)
        {
            // Adicionar o estado Anulada
            db.EstadosDePedido.Add(new EstadoDePedido()
            {
                Pedido = pedido,
                Estado = db.Estados.Where(e => e.Nome == "Anulada").FirstOrDefault(),
                Tempo = DateTime.Now
            });

            return await db.SaveChangesAsync();
        }

        /// <summary>
        /// Insere uma nova localização num pedido.
        /// </summary>
        /// <param name="pedido">Pedido ao qual deve ser adicionada a localização.</param>
        /// <param name="localizacao">Nova localização.</param>
        /// <returns></returns>
        public async Task<int> PostLocalizacao(Pedido pedido, LocalizacaoViewModel localizacao)
        {
            db.Localizacoes.Add(new Localizacao()
            {
                Pedido = pedido,
                // Usar o tempo recebido ou o atual
                Tempo = (localizacao.Tempo == null ? DateTime.Now : (DateTime)localizacao.Tempo),
                Latitude = localizacao.Latitude,
                Longitude = localizacao.Longitude
            });

            return await db.SaveChangesAsync();
        }

        /// <summary>
        /// Devolve a autorização para a aplicação definida.
        /// Devolve null se não for encontrada.
        /// </summary>
        /// <param name="aplicacao">ID da aplicação.</param>
        /// <param name="teste">Define se é uma autorização de teste ou real.</param>
        public Autorizacao GetAutorizacao(Guid aplicacao, bool teste = false)
        {
            return db.Autorizacoes.Where(a => a.Aplicacao.Id == aplicacao && a.Teste == teste && !a.Revogada && a.Validade > DateTime.Now).FirstOrDefault();
        }

        /// <summary>
        /// Libertar recursos.
        /// </summary>
        // Deve ser importante
        public void Dispose()
        {
            db.Dispose();
        }
    }
}