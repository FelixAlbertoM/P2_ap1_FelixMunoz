using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace P2_AP1_FelixMunoz.Models;

public class Pedidos
{
    [Key]
    public int PedidoId { get; set; }

    [Required(ErrorMessage = "Por favor digite la fecha.")]
    public DateTime Fecha { get; set; } =DateTime.Now;

    public string NombreCliente { get; set; }

    public decimal Total {  get; set; }

    [ForeignKey("PedidoId")]
    public virtual List<PedidosDetalle> PedidosDetalle { get; set; } = new List<PedidosDetalle>();
}
