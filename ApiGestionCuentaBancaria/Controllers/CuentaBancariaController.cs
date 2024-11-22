using ApiGestionBancaria.Models;
using ApiGestionBancaria.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;

namespace ApiGestionBancaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaBancariaController : ControllerBase
    {
        private readonly ICuentaBancariaService _cuentaBancariaService;
        public CuentaBancariaController(ICuentaBancariaService cuentaBancariaService)
        {
            _cuentaBancariaService = cuentaBancariaService;
        }

        [HttpPost("CrearCuenta")]
        public IActionResult AddCuentaBancaria([FromBody] CuentaBancaria cuenta)
        {
            try
            {
                var nuevaCuenta = _cuentaBancariaService.AddCuentaBancaria(cuenta);

                return CreatedAtAction(nameof(GetCuentaBancariaById), new { numeroCuenta = nuevaCuenta.NumeroCuenta }, nuevaCuenta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("ObtenerCuenta")]
        public IActionResult GetCuentas()
        {
            var cuentas = _cuentaBancariaService.GetAllCuentaBancaria();
            return Ok(cuentas);
        }
        [HttpGet("Consultar{numeroCuenta}")]
        public IActionResult GetCuentaBancariaById(int numeroCuenta)
        {
            var cuenta = _cuentaBancariaService.GetCuentaBancariaById(numeroCuenta);
            if (cuenta == null)
            {
                return BadRequest("No se encontro ninguna cuenta con ese número");
            }
            else
            {
                return Ok(cuenta);
            }
        }

        [HttpPost("Deposito")]
        public IActionResult AddDeposito([FromBody] Transaccion transaccion)
        {
            try
            {
                var nuevoDeposito = _cuentaBancariaService.AddDeposito(transaccion.NumeroCuenta, transaccion.Monto);

                return Ok(new
                {
                    message = "Depósito realizado con éxito.",
                    transaccion = nuevoDeposito
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Retiro")]
        public IActionResult AddRetiro([FromBody] Transaccion transaccion)
        {
            try
            {
                var nuevoRetiro = _cuentaBancariaService.AddRetiro(transaccion.NumeroCuenta, transaccion.Monto);

                return Ok(new
                {
                    message = "Retiro realizado con éxito.",
                    transaccion = nuevoRetiro
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
