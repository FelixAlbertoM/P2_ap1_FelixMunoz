using System.ComponentModel.DataAnnotations;

namespace P2_AP1_FelixMunoz.Models;

public class PedidoDetalle
{
    [Key]
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int ComponenteId { get; set; }

    public decimal Cantidad { get; set; }

    public decimal Precio { get; set; }

}
