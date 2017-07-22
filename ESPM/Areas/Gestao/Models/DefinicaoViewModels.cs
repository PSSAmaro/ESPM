using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESPM.Areas.Gestao.Models
{
    public class ValorDefinicaoViewModel<T>
    {
        T Valor { get; set; }

        public static implicit operator T(ValorDefinicaoViewModel<T> valor)
        {
            return valor.Valor;
        }
    }
}