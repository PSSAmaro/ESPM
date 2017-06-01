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
        // Id da aplicação
        public Guid Aplicacao { get; set; }

        // Hash criado com a chave da aplicação
        public string Hash { get; set; }

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
        public string OutrosDetalhesPessoa { get; set; }

        // Descrição da situação
        public string Descricao { get; set; }

        // Latitude da localização detetada
        public float? Latitude { get; set; }

        // Longitude da localização detetada
        public float? Longitude { get; set; }

        public override string ToString()
        {
            // Modificar se o modelo for modificado
            // Ignorar tempo porque pode ser modificado no servidor
            string str = Nome + Contacto + Idade + OutrosDetalhesPessoa + Descricao + Latitude + Longitude;

            // Remover caracteres que podem ser ambíguos, procurar outros
            foreach (string c in new string[] { ",", "." })
                str = str.Replace(c, string.Empty);

            return str;
        }
    }

    public class RecebidoViewModel
    {
        // Guid a usar para obter informação de estado
        public Guid Id { get; set; }

        // Momento em que o pedido foi recebido
        public DateTime Recebido { get; set; }

        // Possível aviso a enviar, sem uso neste momento
        // public string Aviso { get; set; }
    }

    public class EstadoAtualViewModel
    {
        // Nome do estado atual do pedido
        public string Estado { get; set; }

        // Momento em que o pedido foi modificado para este estado
        public DateTime Modificado { get; set; }
    }
}