using ESPM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ESPM.Filters
{
    public class Validacao : ActionFilterAttribute
    {
        public bool Teste { get; set; }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                bool formatoValido = true;

                if (filterContext.ActionArguments.Any(a => a.Value == null) || !filterContext.ModelState.IsValid)
                {
                    formatoValido = false;
                    filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Formato inválido.");
                }
                else if (!PedidoExiste(filterContext.ActionDescriptor.ActionName, filterContext.ActionArguments, db))
                    filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, "O pedido não existe.");

                if (filterContext.ActionDescriptor.ActionName == "Get")
                    return;

                AvaliacaoPedido avaliacao = new AvaliacaoPedido()
                {
                    Tempo = DateTime.Now,
                    Endereco = db.Enderecos.Find((string)filterContext.Request.Properties["Endereco"]),
                    Resultado = Resultado.NaoAvaliado
                };

                IEnumerable<string> header;
                if (filterContext.Request.Headers.TryGetValues("X-ESPM-Autenticacao", out header))
                {
                    Guid guid;
                    Guid.TryParse(header.FirstOrDefault(), out guid);
                    avaliacao.Header = guid;
                }

                // https://stackoverflow.com/questions/32339283/webapi-get-the-post-raw-body-inside-a-filter
                using (var stream = new StreamReader(filterContext.Request.Content.ReadAsStreamAsync().Result))
                {
                    stream.BaseStream.Position = 0;
                    avaliacao.Corpo = stream.ReadToEnd();
                }

                if (!formatoValido)
                    avaliacao.Resultado = Resultado.MauFormato;
                else if (!db.Autorizacoes.Any(a => a.Id == avaliacao.Header && a.Teste == Teste && !a.Revogada && a.Validade > DateTime.Now) && MesmoEnderecoAutorizacao(filterContext.ActionDescriptor.ActionName, filterContext.ActionArguments, avaliacao, db))
                {
                    // Processar o pedido à mesma e não alertar o utilizador
                    avaliacao.Resultado = Resultado.ErroAutenticacao;
                }
                else if(!InfoSuficiente(filterContext.ActionDescriptor.ActionName, filterContext.ActionArguments))
                {
                    // Guardar pedido à mesma, mas provavelmente não existem dados suficientes para tratar
                    avaliacao.Resultado = Resultado.DadosInsuficientes;
                }
                else
                    avaliacao.Resultado = Resultado.Valido;

                // FALTA: Ver pedidos suspeitos ou repetidos
                // Exemplo suspeitos: Localização fora, idade muito alta
                // Exemplo repetidos: Mesmo IP não livre em poucos segundos/minutos, todas as informações iguais

                // Adicionar avaliação à BD
                db.Avaliacoes.Add(avaliacao);
                db.SaveChanges();

                // Colocar avaliação nas propriedades do pedido para permitir acesso pelo controlador
                filterContext.Request.Properties.Add("Avaliacao", avaliacao.Id);
            }
        }

        private bool PedidoExiste(string metodo, Dictionary<string, object> argumentos, ApplicationDbContext db)
        {
            if (metodo == "Post")
                return true;
            Guid id = (Guid)argumentos["id"];
            if (db.Pedidos.Any(p => p.Id == id))
                return true;
            return false;
        }

        private bool InfoSuficiente(string metodo, Dictionary<string, object> argumentos)
        {
            if (metodo == "Delete")
                return true;
            EmergenciaViewModel emergencia = (EmergenciaViewModel)argumentos["emergencia"];
            if (emergencia.Contacto == null && emergencia.OutrosDetalhesPessoa == null && emergencia.Descricao == null && emergencia.Localizacoes.Count == 0)
                return false;
            return true;
        }

        private bool MesmoEnderecoAutorizacao(string metodo, Dictionary<string, object> argumentos, AvaliacaoPedido avaliacao, ApplicationDbContext db)
        {
            if (metodo == "Post")
                return true;
            Pedido pedido = db.Pedidos.Find((Guid)argumentos["id"]);
            // Cuidado com esta utilização porque o IP dos telemóveis muda facilmente
            /*if (pedido.Autorizacao.Id == avaliacao.Header && pedido.Avaliacao.Endereco == avaliacao.Endereco)
                return true;
            return false;*/
            return true;
        }
    }
}