using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CamposDealer.Models
{
    public class Produto
    {
        public int idProduto { get; set; }

        [Display(Name = "Descrição do produto")]
        public string dscProduto { get; set; } = string.Empty;

        [Display(Name = "Valor únitário do produto")]
        public decimal vlrUnitario { get; set; }
    }


}
