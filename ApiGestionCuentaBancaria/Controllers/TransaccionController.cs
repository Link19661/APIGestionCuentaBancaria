using ApiGestionBancaria.Models;
using ApiGestionBancaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionBancaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly ITransaccionService _transaccionService;

        public TransaccionController(ITransaccionService transaccionService)
        {
            _transaccionService = transaccionService;
        }
        [HttpGet("Consultar{numeroCuenta}")]
        public IActionResult GetTransaccionesByCuenta(int numeroCuenta)
        {
            var transacciones = _transaccionService.GetTransaccionesByCuenta(numeroCuenta);

            if (transacciones == null)
            {
                return NotFound(new { message = "No se encontró ninguna transacción para la cuenta" });
            }
            else
            {
                return Ok(transacciones);
            }
        }
    }
}
