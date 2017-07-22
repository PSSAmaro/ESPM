using ESPM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ESPM.Areas.Gestao.Controllers
{
    public class DefinicoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.Definicoes.Where(d => d.Listar).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(FormCollection form)
        {
            List<string> erros = new List<string>();
            foreach (string key in form.AllKeys)
            {
                Definicao d = await db.Definicoes.FindAsync(key);
                if (d != null)
                {
                    try
                    {
                        // Ler o valor do Form
                        string value = form[key];
                        int valor;
                        // Se for true é recebido "true,false"
                        if (value.Contains("true"))
                            valor = 1;
                        else if (value == "false")
                            valor = 0;
                        else
                            valor = Int32.Parse(value);

                        // Tentar alterar a definição
                        switch (d.Alterar(valor, System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId())))
                        {
                            case -2:
                                // Ocorreu um erro
                                erros.Add(d.Apresentacao);
                                break;
                            case -1:
                                // Nada foi alterado
                                break;
                            default:
                                // Guardar a cada para não perder alterações válidas
                                await db.SaveChangesAsync();
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        // Se o formato dos campos enviados estiver incorreto
                        erros.Add(d.Apresentacao);
                    }
                        
                }
            }
            if (erros.Count == 0)
            {
                ViewBag.Mensagem = "As definições foram alterados com sucesso.";
                ViewBag.Classe = "text-success";
            }
            else
            {
                ViewBag.Mensagem = "Algumas definições não foram alteradas com sucesso: ";
                bool p = true;
                foreach (string s in erros)
                {
                    if (p)
                        p = false;
                    else
                        ViewBag.Mensagem += ", ";
                    ViewBag.Mensagem += s;
                }
                ViewBag.Classe = "text-danger";
            }
            return View(await db.Definicoes.Where(d => d.Listar).ToListAsync());
        }
    }
}