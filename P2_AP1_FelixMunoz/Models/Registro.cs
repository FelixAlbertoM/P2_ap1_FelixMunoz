using System.ComponentModel.DataAnnotations;
namespace P2_AP1_FelixMunoz.Models;

public class Registro
{
    [Key]
    public int IdRegistro { get; set; }

    [Required(ErrorMessage = "Por favor digite la fecha.")]
    public DateTime Fecha { get; set; } =DateTime.Now;

}
