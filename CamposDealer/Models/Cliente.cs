using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace CamposDealer.Models
{
    public class Cliente
    {

        public int idCliente { get; set; }

        [Display(Name = "Nome do Cliente")]
        public string nmCliente { get; set; } = string.Empty;

        [Display(Name = "Nome da Cidade")]
        public string cidade { get; set; } = string.Empty;
    }
}
