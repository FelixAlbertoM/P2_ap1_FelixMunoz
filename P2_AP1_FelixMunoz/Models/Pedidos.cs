using System.ComponentModel.DataAnnotations;
namespace P2_AP1_FelixMunoz.Models;

public class Pedidos
{
    [Key]
    public int PedidoId { get; set; }

    [Required(ErrorMessage = "Por favor digite la fecha.")]
    public DateTime Fecha { get; set; } =DateTime.Now;

    public string NombreCliente { get; set; }

    public decimal Total {  get; set; }

}
