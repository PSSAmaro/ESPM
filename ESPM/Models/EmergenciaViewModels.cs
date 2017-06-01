using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    // Este é o modelo dos pedidos enviados, provavelmente vai ser modificado ainda
    // Por enquanto suporta 1 localização e não suporta imagens
    public class EmergenciaViewModel
    {
        // Chave privada da aplicação
        public Guid Chave { get; set; }

        // Momento em que o pedido foi enviado
        // Talvez mudar para unix time?
        public DateTime? Tempo { get; set; }

        // Nome da pessoa que precisa de ajuda
        public string Nome { get; set; }

        // Contacto da pessoa que precisa de ajuda
        public int? Contacto { get; set; }

        // Idade da pessoa que precisa de ajuda
        public int? Idade { get; set; }

        // Outros detalhes da condição da pessoa que precisa de ajuda
        public string Condicao { get; set; }

        // Descrição da situação
        public string Descricao { get; set; }

        // Latitude da localização detetada
        public float? Latitude { get; set; }

        // Longitude da localização detetada
        public float? Longitude { get; set; }
    }

    public class RecebidoViewModel
    {
        // Guid a usar para obter informação de estado
        public Guid Id { get; set; }

        // Momento em que o pedido foi recebido
        public DateTime Recebido { get; set; }

        // Possível aviso a enviar, sem uso neste momento
        public string Aviso { get; set; }
    }

    public class EstadoAtualViewModel
    {
        // Nome do estado atual do pedido
        public string Estado { get; set; }

        // Momento em que o pedido foi modificado para este estado
        public DateTime Modificado { get; set; }
    }
}