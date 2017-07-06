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
                // Não é necessária validação para o método Get
                if (filterContext.ActionDescriptor.ActionName == "Get")
                    return;

                // Criar a nova avaliação
                Avaliacao avaliacao = new Avaliacao()
                {
                    Tempo = DateTime.Now,
                    Endereco = db.Enderecos.Find((string)filterContext.Request.Properties["Endereco"]),
                    Resultado = Resultado.NaoAvaliado,
                    Header = Guid.Empty
                };

                // Registar header de autenticação do pedido
                IEnumerable<string> header;
                if (filterContext.Request.Headers.TryGetValues("X-ESPM-Autenticacao", out header))
                {
                    Guid guid = Guid.Empty;
                    Guid.TryParse(header.FirstOrDefault(), out guid);
                    avaliacao.Header = guid;
                }

                // Registar corpo do pedido
                // https://stackoverflow.com/questions/32339283/webapi-get-the-post-raw-body-inside-a-filter
                using (var stream = new StreamReader(filterContext.Request.Content.ReadAsStreamAsync().Result))
                {
                    stream.BaseStream.Position = 0;
                    avaliacao.Corpo = stream.ReadToEnd();
                }

                // Verificar se o formato é válido
                if (filterContext.ActionArguments.Any(a => a.Value == null) || !filterContext.ModelState.IsValid)
                {
                    // Neste caso o pedido não é tratado
                    avaliacao.Resultado = Resultado.MauFormato;
                    // Responder com BadRequest
                    filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Formato inválido.");
                }
                else if (!db.Autorizacoes.Any(a => a.Id == avaliacao.Header && a.Teste == Teste && !a.Revogada && a.Validade > DateTime.Now))
                {
                    // Processar o pedido à mesma e não alertar o utilizador
                    // Talvez enviar um alerta para o responsável pela aplicação
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

        private bool InfoSuficiente(string metodo, Dictionary<string, object> argumentos)
        {
            switch (metodo)
            {
                case "Delete":
                    return true;
                case "Post":
                    EmergenciaViewModel emergencia = (EmergenciaViewModel)argumentos["emergencia"];
                    if (emergencia.Contacto == null && emergencia.OutrosDetalhesPessoa == null && emergencia.Descricao == null && emergencia.Localizacoes.Count == 0)
                        return false;
                    return true;
                case "Put":
                    AtualizacaoViewModel atualizacao = (AtualizacaoViewModel)argumentos["atualizacao"];
                    if (atualizacao.Descricao == null && atualizacao.Localizacoes.Count == 0)
                        return false;
                    return true;
                default:
                    return false;
            }
        }
    }
}