using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiGestionBancaria.Models
{
    public class Transaccion
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public required int NumeroCuenta { get; set; }

        public required string TipoTransaccion { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El saldo debe ser un valor positivo.")]
        public required decimal Monto { get; set; }

        public decimal SaldoDisponible { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
