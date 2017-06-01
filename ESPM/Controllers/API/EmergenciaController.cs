using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ESPM.Controllers.API
{
    public class EmergenciaController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<IHttpActionResult> Get(Guid id)
        {
            Pedido pedido = await db.Pedidos.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (pedido == null)
                return NotFound();
            return Ok(new EstadoAtualViewModel() {
                Estado = pedido.EstadoAtual().Estado.Nome,
                Modificado = pedido.EstadoAtual().Tempo
            });
        }

        public async Task<IHttpActionResult> Post(EmergenciaViewModel emergencia)
        {
            if (ModelState.IsValid)
            {
                // Usar qualquer coisa assim para o hash da chave
                // SHA1 sha = new SHA1CryptoServiceProvider();
                // string result = sha.ComputeHash(Encoding.UTF8.GetBytes(emergencia.OutrosDetalhesPessoa)).ToString();

                // Momento em que o pedido foi recebido
                DateTime recebido = DateTime.Now;

                // Depois mudar isto com o hash e isso...
                Autorizacao aut = db.Autorizacoes.Where(a => !a.Teste && !a.Revogada && a.Aplicacao.Id == emergencia.Aplicacao && (a.Validade == null || a.Validade < recebido)).FirstOrDefault();
                if (aut == null)
                    return BadRequest("A chave utilizada não é válida");

                // Validação do pedido aqui
                // Apesar do modelo válido convém confirmar que foram enviadas informações suficientes
                // Devolver BadRequest caso contrário

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
                    Id = pedido.Id,
                    Recebido = recebido
                });
            }
            return BadRequest("O pedido enviado não tem um formato válido");
        }

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
