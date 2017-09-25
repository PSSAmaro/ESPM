using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ESPM.Helpers
{
    // É PARA REMOVAR O MAIS RÁPIDO POSSÍVEL
    /// <summary>
    /// Gere a criação e edição dos pedidos.
    /// </summary>
    // Toda esta classe provavelmente devia ser implementada com os get e set do C#, mas eu sou mau e nem procurei se dava.
    // FALTA: IMAGENS!!!!!!!!
    // Isto vai ter que levar um refactoring a sério, tá muito miserável
    public class GestorPedidos
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Carrega todos os pedidos da BD.
        /// </summary>
        /// <returns>Todos os pedidos existentes na BD.</returns>
        public IQueryable<Pedido> Pedidos()
        {
            return db.Pedidos;
        }

        /// <summary>
        /// Carrega todos os pedidos abertos na BD.
        /// </summary>
        /// <returns>Devolve os pedidos abertos.</returns>
        public IQueryable<Pedido> PedidosAbertos()
        {
            // O que a seguinte query faz é procurar pedidos cujo estado ainda tenha estados seguintes
            return db.Pedidos.Where(p => p.Estados.OrderByDescending(e => e.Tempo).FirstOrDefault().Estado.Seguintes.Count > 0);
        }

        /// <summary>
        /// Carrega todos os pedidos abertos na BD.
        /// </summary>
        /// <returns>Devolve os pedidos abertos.</returns>
        public IQueryable<Pedido> PedidosFechados()
        {
            // O que a seguinte query faz é procurar pedidos cujo estado não tenha estados seguintes
            return db.Pedidos.Where(p => p.Estados.OrderByDescending(e => e.Tempo).FirstOrDefault().Estado.Seguintes.Count == 0);
        }

        /// <summary>
        /// Devolve o pedido com o id definido.
        /// Devolve null se não for encontrado.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        public async Task<Pedido> Pedido(Guid id)
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
        public async Task<Pedido> CriarPedido(EmergenciaViewModel emergencia, Autorizacao autorizacao)
        {
            // Usar o tempo recebido ou o atual
            DateTime t = (emergencia.Tempo == null ? DateTime.Now : (DateTime)emergencia.Tempo);
            Pedido pedido = new Pedido()
            {
                Autorizacao = autorizacao,
                Tempo = t,
                Modificado = t
            };
            
            // Adicionar descrição e localizações se existirem
            if (emergencia.Descricao != null)
                pedido.Descricoes.Add(await CriarDescricao(emergencia.Descricao, t));
            if (emergencia.Localizacoes != null)
                pedido.Localizacoes.AddRange(await AdicionarLocalizacoes(emergencia.Localizacoes));
            // Adicionar o estado inicial
            pedido.Estados.Add(await AlterarEstadoDePedido("Inicial"));

            db.Pedidos.Add(pedido);
            await db.SaveChangesAsync();

            return pedido;
        }

        /// <summary>
        /// Atualizar um pedido.
        /// </summary>
        /// <param name="pedido">Pedido a atualizar.</param>
        /// <param name="atualizacao">Atualização com as informações.</param>
        /// <returns></returns>
        /*public async Task<Pedido> AtualizarPedido(Pedido pedido, AtualizacaoViewModel atualizacao)
        {
            // Usar o tempo recebido ou o atual
            DateTime t = (atualizacao.Tempo == null ? DateTime.Now : (DateTime)atualizacao.Tempo);
            if (atualizacao.Descricao != null)
                pedido.Descricoes.Add(await CriarDescricao(atualizacao.Descricao, t));
            if (atualizacao.Localizacoes != null)
                pedido.Localizacoes.AddRange(await AdicionarLocalizacoes(atualizacao.Localizacoes));

            // Falta testar
            db.Entry(pedido).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return pedido;
        }*/

        /// <summary>
        /// Marca um pedido como anulado.
        /// </summary>
        /// <param name="pedido">Pedido a anular.</param>
        /// <returns></returns>
        public async Task<EstadoDePedido> CancelarPedido(Pedido pedido)
        {
            // Adicionar o estado Anulada
            return await AlterarEstadoDePedido("Anulada", pedido);
        }

        /// <summary>
        /// Insere uma ou mais novas localizações num pedido.
        /// </summary>
        /// <param name="localizacoes">Nova localização.</param>
        /// <param name="pedido">Pedido ao qual deve ser adicionada a localização.</param>
        /// <returns></returns>
        public async Task<List<Localizacao>> AdicionarLocalizacoes(List<LocalizacaoViewModel> localizacoes, Pedido pedido = null)
        {
            List<Localizacao> novas = new List<Localizacao>();
            foreach (var localizacao in localizacoes)
            {
                novas.Add(new Localizacao()
                {
                    // Pedido = pedido,
                    // Usar o tempo recebido ou o atual
                    Tempo = (localizacao.Tempo == null ? DateTime.Now : (DateTime)localizacao.Tempo),
                    Avaliacao = new Avaliacao()
                    {
                        Tempo = DateTime.Now,
                        Resultado = Resultado.Valido
                    },
                    Latitude = localizacao.Latitude,
                    Longitude = localizacao.Longitude
                });
            }

            if (pedido != null)
            {
                db.Localizacoes.AddRange(novas);
                await db.SaveChangesAsync();
            }

            return novas;
        }

        /// <summary>
        /// Cria um novo estado para um pedido.
        /// </summary>
        /// <param name="estado">Novo estado do pedido.</param>
        /// <param name="pedido">Pedido ao qual adicionar o estado.</param>
        /// <returns></returns>
        // Palavra especial: Inicial - Altera para o estado definido como inicial
        public async Task<EstadoDePedido> AlterarEstadoDePedido(string estado, Pedido pedido = null)
        {
            Estado seguinte;
            if (estado == "Inicial")
                seguinte = db.Estados.Find(db.Definicoes.Find("EstadoInicial").Valor);
            else
                seguinte = db.Estados.Where(e => e.Nome == estado).FirstOrDefault();

            EstadoDePedido novo = new EstadoDePedido()
            {
                Pedido = pedido,
                Estado = seguinte,
                Tempo = DateTime.Now
            };

            if (pedido != null)
            {
                Estado atual = pedido.Estados.OrderByDescending(e => e.Tempo).FirstOrDefault().Estado;

                // Validar a transição
                if (db.TransicoesDeEstado.Where(t => t.De == atual && t.Para == seguinte).ToList().Count == 1)
                {
                    db.EstadosDePedido.Add(novo);
                    await db.SaveChangesAsync();
                }
                else
                    return null;
            }
            
            return novo;
        }

        /// <summary>
        /// Cria uma nova descrição para um pedido.
        /// </summary>
        /// <param name="texto">Texto da descrição.</param>
        /// <param name="tempo">Momento em que a descrição foi escrita.</param>
        /// <param name="pedido">Pedido ao qual deve ser adicionada a descrição.</param>
        /// <returns></returns>
        public async Task<Descricao> CriarDescricao(string texto, DateTime tempo, Pedido pedido = null)
        {
            if (texto == null)
                return null;

            Descricao descricao = new Descricao()
            {
                Pedido = pedido,
                Tempo = tempo,
                Texto = texto
            };

            if (pedido != null)
            {
                db.Descricoes.Add(descricao);
                await db.SaveChangesAsync();
            }

            return descricao;
        }

        /// <summary>
        /// Devolve a autorização para a aplicação definida.
        /// Devolve null se não for encontrada.
        /// </summary>
        /// <param name="aplicacao">ID da aplicação.</param>
        /// <param name="teste">Define se é uma autorização de teste ou real.</param>
        public Autorizacao Autorizacao(Guid aplicacao, bool teste = false)
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