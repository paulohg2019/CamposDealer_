using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CamposDealer.Models
{
    public class Venda
    {
        public int idVenda { get; set; }

        [ForeignKey("Cliente")]
        [Display(Name = "Nome do cliente")]
        public int idCliente { get; set; }

        [ForeignKey("Produto")]
        [Display(Name = "Descrição do produto")]
        public int idProduto { get; set; }

        [Display(Name = "quantidade de venda")]
        public int qtdVenda { get; set; }

        [Display(Name = "Valor unitário da venda")]
        public decimal vlrUnitarioVenda { get;  set; }

        [Display(Name = "Data da venda")]
        public DateTime dthVenda { get; set; }

        [Display(Name = "Valor total da venda")]
        public decimal vlrTotalVenda => qtdVenda * vlrUnitarioVenda;


        public virtual Cliente Cliente { get; set; } = new Cliente();
        public virtual Produto Produto { get; set; } = new Produto();
    }


}
