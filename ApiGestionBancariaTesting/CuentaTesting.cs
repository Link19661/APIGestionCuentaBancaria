using ApiGestionBancaria.Controllers;
using ApiGestionBancaria.Data;
using ApiGestionBancaria.Models;
using ApiGestionBancaria.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionBancariaTesting
{
    public class CuentaTesting
    {
        private readonly CuentaBancariaController _controller;
        private readonly ICuentaBancariaService _cuentaBancariaService;
        private readonly AppDbContext _context;

        public CuentaTesting()
        {
            // Configurar la base de datos en memoria
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new AppDbContext(options);
            _cuentaBancariaService = new CuentaBancariaService(_context);
            _controller = new CuentaBancariaController(_cuentaBancariaService);
        }
        [Fact]
        //Prueba creación de cuenta bancaria + generación y asignación de # de cuenta
        public void Create_Ok()
        {
            var nuevaCuenta = new CuentaBancaria
            {
                Nombre = "Jeremy Silva",  
                Saldo = 1000 
            };

            var resultado = _controller.AddCuentaBancaria(nuevaCuenta) as CreatedAtActionResult;

            Assert.NotNull(resultado); 
            Assert.IsType<CreatedAtActionResult>(resultado);

            var cuentaCreada = resultado.Value as CuentaBancaria;
            Assert.NotNull(cuentaCreada);
            Assert.Equal("Jeremy Silva", cuentaCreada.Nombre);
            Assert.Equal(1000, cuentaCreada.Saldo); 

            Assert.True(cuentaCreada.NumeroCuenta > 0, "El número de cuenta debe ser mayor que 0");
        }

        [Fact]
        //Prueba de depósito (Validación de sumatoria)
        public void AddDeposito_Correctamente()
        {
            var cuenta = new CuentaBancaria
            {
                Nombre = "Jeremy Silva",
                Saldo = 1000 
            };

            _controller.AddCuentaBancaria(cuenta); 

            decimal montoDeposito = 500;

            var transaccion = _cuentaBancariaService.AddDeposito(cuenta.NumeroCuenta, montoDeposito);

            var cuentaActualizada = _context.CuentaBancarias.FirstOrDefault(c => c.NumeroCuenta == cuenta.NumeroCuenta);
            Assert.NotNull(cuentaActualizada); 
            Assert.Equal(1500, cuentaActualizada.Saldo);

            var transacciones = _context.Transacciones.Where(t => t.NumeroCuenta == cuenta.NumeroCuenta).ToList();
            Assert.NotEmpty(transacciones); 
            var ultimaTransaccion = transacciones.Last(); 

            Assert.Equal("Depósito", ultimaTransaccion.TipoTransaccion);

            Assert.Equal(montoDeposito, ultimaTransaccion.Monto);

            Assert.Equal(1500, ultimaTransaccion.SaldoDisponible);
        }

        [Fact]
        //Verificación de cuenta no encontrada
        public void AddDeposito_CuentaNoEncontrada()
        {
            int numeroCuentaInexistente = 9999;
            decimal montoDeposito = 500;

            var exception = Assert.Throws<Exception>(() => _cuentaBancariaService.AddDeposito(numeroCuentaInexistente, montoDeposito));
            Assert.Equal("No se encontró la cuenta con el número 9999.", exception.Message);
        }

        [Fact]
        //Prueba de retiro realizado + confirmación de saldo
        public void AddRetiro_RetiroExitoso()
        {
            var cuenta = new CuentaBancaria
            {
                Nombre = "Jeremy Silva",
                Saldo = 1000
            };

            _controller.AddCuentaBancaria(cuenta);

            decimal montoRetiro = 500;

            var transaccion = _cuentaBancariaService.AddRetiro(cuenta.NumeroCuenta, montoRetiro);

            var cuentaActualizada = _context.CuentaBancarias.FirstOrDefault(c => c.NumeroCuenta == cuenta.NumeroCuenta);
            Assert.NotNull(cuentaActualizada); 
            Assert.Equal(500, cuentaActualizada.Saldo); 

            var transacciones = _context.Transacciones.Where(t => t.NumeroCuenta == cuenta.NumeroCuenta).ToList();
            Assert.NotEmpty(transacciones); 
            var ultimaTransaccion = transacciones.Last();

            Assert.Equal("Retiro", ultimaTransaccion.TipoTransaccion);

            Assert.Equal(montoRetiro, ultimaTransaccion.Monto);

            Assert.Equal(500, ultimaTransaccion.SaldoDisponible); 
        }

        [Fact]
        //Prueba de fondos insuficientes
        public void AddRetiro_FondosInsuficientes()
        {
            var cuenta = new CuentaBancaria
            {
                Nombre = "Jeremy Silva",    
                Saldo = 200
            };

            _controller.AddCuentaBancaria(cuenta); 

            decimal montoRetiro = 500;

            var exception = Assert.Throws<Exception>(() => _cuentaBancariaService.AddRetiro(cuenta.NumeroCuenta, montoRetiro));
            Assert.Equal("No se puede realizar el retiro por Fondos insuficientes.", exception.Message);
        }

        [Fact]
        //Prueba de cuenta no encontrada al tratar de realizar el retiro
        public void AddRetiro_CuentaNoEncontrada()
        {
            int numeroCuentaInexistente = 9999;
            decimal montoRetiro = 500;

            var exception = Assert.Throws<Exception>(() => _cuentaBancariaService.AddRetiro(numeroCuentaInexistente, montoRetiro));
            Assert.Equal("No se encontró la cuenta con el número 9999.", exception.Message);
        }

    }
}