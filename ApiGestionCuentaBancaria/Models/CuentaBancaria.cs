using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace ApiGestionBancaria.Models
{
    public class CuentaBancaria
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        public int NumeroCuenta { get; set; }

        [MaxLength(100)]
        public required string Nombre { get; set; }

        //[Range(100, double.MaxValue, ErrorMessage = "El saldo inicial debe ser un valor positivo.")]
        public required decimal Saldo { get; set; }

    }
}
