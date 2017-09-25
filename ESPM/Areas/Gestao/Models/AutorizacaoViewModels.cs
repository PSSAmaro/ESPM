using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESPM.Areas.Gestao.Models
{
    public class AutorizacaoViewModel
    {
        public Guid Id { get; set; }

        public DateTime Validade { get; set; }

        public string Utilizador { get; set; }

        public string Aplicacao { get; set; }
    }
}