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
                // Momento em que o pedido foi recebido
                DateTime recebido = DateTime.Now;

                // Verificar se a chave utilizada é válida (Confirmar se a comparação com o null funciona)
                if(db.Autorizacoes.Where(a => !a.Teste && !a.Revogada && a.Id == emergencia.Chave && (a.Validade == null || a.Validade < recebido)).Count() != 1)
                    return BadRequest("A chave utilizada não é válida");

                // Validação do pedido aqui
                // Apesar do modelo válido convém confirmar que foram enviadas informações suficientes
                // Devolver BadRequest caso contrário

                // Criar o novo pedido
                Pedido pedido = new Pedido();

                // Associar a autorização usada
                pedido.Autorizacao = db.Autorizacoes.Where(a => a.Id == emergencia.Chave).FirstOrDefault();

                // Usar o tempo recebido ou o atual
                if (emergencia.Tempo != null)
                    pedido.Tempo = (DateTime)emergencia.Tempo;
                else
                    pedido.Tempo = recebido;

                // Será a melhor maneira?
                // Ver a questão dos int? e das strings vazias
                // Adicionar pessoa se existirem informações de pessoa
                if (emergencia.Nome != null || emergencia.Contacto != null || emergencia.Idade != null || emergencia.Condicao != null)
                {
                    pedido.Pessoa = new Pessoa()
                    {
                        Nome = emergencia.Nome,
                        Contacto = emergencia.Contacto,
                        Idade = emergencia.Idade,
                        Condicao = emergencia.Condicao
                    };
                }

                // Adicionar descrição se existir descrição
                if(emergencia.Descricao != null)
                {
                    pedido.Descricoes.Add(new Descricao()
                    {
                        Tempo = pedido.Tempo,
                        Texto = emergencia.Descricao
                    });
                }

                // Adicionar localização se existir localização
                if(emergencia.Latitude != null && emergencia.Longitude != null)
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
