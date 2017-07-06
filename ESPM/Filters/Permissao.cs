using ESPM.Models;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ESPM.Filters
{
    /// <summary>
    /// Verifica se o dispositivo que enviou o pedido não é um da blacklist.
    /// Uma vez que este filtro é o primeiro a correr, impede abusadores comuns.
    /// Não deve ser usada para autenticação, uma vez que pedido não autenticados devem passar à mesma,
    /// ficando apenas na lista de suspeitos.
    /// </summary>
    public class Permissao : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Validar dispositivo que enviou o pedido.
        /// </summary>
        /// <param name="filterContext">Contexto do pedido.</param>
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            // Ignorar o método Get
            if (filterContext.ActionDescriptor.ActionName == "Get")
                return;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string atual = EnderecoIP(filterContext.Request);
                Endereco IP = db.Enderecos.Find(atual);
                if (IP == null)
                {
                    IP = new Endereco()
                    {
                        IP = atual,
                        Livre = false,
                        Limite = db.Definicoes.Find("LimitePedidosFalsosIP").Valor
                    };
                    db.Enderecos.Add(IP);
                    db.SaveChanges();
                }
                else if (!IP.Livre && IP.Falsos >= IP.Limite)
                {
                    // Os IP bloqueados são completamente ignorados, para evitar ataques DoS
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    return;
                }
                // Colocar endereço nas propriedades do pedido para permitir acesso pela validação e pelo controlador
                filterContext.Request.Properties.Add("Endereco", IP.IP);
            }
        }

        // https://stackoverflow.com/questions/40992485/how-to-get-authenticated-users-name-ip-address-and-the-controller-action-bein
        private string EnderecoIP(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
                return ((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            if (request.Properties.ContainsKey("MS_OwinContext"))
                return ((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress;
            return string.Empty;
        }
    }
}