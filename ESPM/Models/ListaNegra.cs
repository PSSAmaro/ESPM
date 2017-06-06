using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;

namespace ESPM.Models
{
    public class ListaNegra
    {
        // IP "marcado"
        [Key]
        public IPAddress IP { get; set; }

        // Número de pedidos falsos já realizados por este IP
        // Se este número for maior ou igual a Limite, marcar o pedido como falso
        public int PedidosFalsos { get; set; }

        // Limite para este IP
        // Todos começam com o mesmo limite, que é uma definição editável pelos administradores
        public int Limite { get; set; }
    }
}