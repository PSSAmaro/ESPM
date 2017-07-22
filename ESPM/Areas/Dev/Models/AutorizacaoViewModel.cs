using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESPM.Areas.Dev.Models
{
    public class AutorizacaoViewModel
    {
        public Guid Id { get; set; }

        public string Aplicacao { get; set; }

        public DateTime Validade { get; set; }

        public bool Teste { get; set; }
    }
}