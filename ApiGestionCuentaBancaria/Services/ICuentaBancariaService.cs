using ApiGestionBancaria.Data;
using ApiGestionBancaria.Models;
using System;

namespace ApiGestionBancaria.Services
{
    public interface ICuentaBancariaService
    {
        CuentaBancaria AddCuentaBancaria(CuentaBancaria cuenta);
        IEnumerable<CuentaBancaria> GetAllCuentaBancaria();
        CuentaBancaria GetCuentaBancariaById(int numeroCuenta);
        Transaccion AddDeposito(int numeroCuenta, decimal monto);
        Transaccion AddRetiro(int numeroCuenta, decimal monto);
    }
    public class CuentaBancariaService : ICuentaBancariaService
    {
        private readonly AppDbContext _appDbContext;
        public CuentaBancariaService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        //Creación de cuenta y depósito inicial
        public CuentaBancaria AddCuentaBancaria(CuentaBancaria cuenta)
        {
            cuenta.NumeroCuenta = GenerarNumeroCuenta();
            if (_appDbContext.CuentaBancarias.Any(c => c.NumeroCuenta == cuenta.NumeroCuenta))
            {
                throw new Exception("Ya existe una cuenta con ese número.");
            }

            if (cuenta.Saldo < 100)
            {
                throw new Exception("El saldo inicial debe ser mayor o igual a 100.");
            }

            // Registrar depósito inicial
            var transaccionInicial = new Transaccion
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                TipoTransaccion = "Depósito Inicial",
                Monto = cuenta.Saldo,
                SaldoDisponible = cuenta.Saldo,
                Fecha = DateTime.UtcNow.Date + DateTime.UtcNow.TimeOfDay
            };

            _appDbContext.CuentaBancarias.Add(cuenta);
            _appDbContext.Transacciones.Add(transaccionInicial);
            _appDbContext.SaveChanges();

            return cuenta;
        }
        public IEnumerable<CuentaBancaria> GetAllCuentaBancaria()
        {
            return _appDbContext.CuentaBancarias.ToList();
        }
        public CuentaBancaria GetCuentaBancariaById(int numeroCuenta)
        {
            return _appDbContext.CuentaBancarias.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);
        }
        //Transacción de depósito
        public Transaccion AddDeposito(int numeroCuenta, decimal monto)
        {
            var cuenta = _appDbContext.CuentaBancarias.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null)
            {
                throw new Exception($"No se encontró la cuenta con el número {numeroCuenta}.");
            }
            if (monto < 100)
            {
                throw new Exception("El depósito debe ser mayor o igual a 100.");
            }
            cuenta.Saldo += monto;

            var transaccion = new Transaccion
            {
                NumeroCuenta = numeroCuenta,
                TipoTransaccion = "Depósito",
                Monto = monto,
                SaldoDisponible = cuenta.Saldo,
                Fecha = DateTime.UtcNow.Date + DateTime.UtcNow.TimeOfDay
            };

            _appDbContext.Transacciones.Add(transaccion);
            _appDbContext.SaveChanges();

            return transaccion;
        }
        //Transacción de retiro
        public Transaccion AddRetiro(int numeroCuenta, decimal monto)
        {
            var cuenta = _appDbContext.CuentaBancarias.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null)
            {
                throw new Exception($"No se encontró la cuenta con el número {numeroCuenta}.");
            }
            if (cuenta.Saldo < monto)
            {
                throw new Exception($"No se puede realizar el retiro por Fondos insuficientes.");
            }
            if (monto < 100)
            {
                throw new Exception("El retiro debe ser mayor o igual a 100.");
            }
            cuenta.Saldo -= monto;

            var transaccion = new Transaccion
            {
                NumeroCuenta = numeroCuenta,
                TipoTransaccion = "Retiro",
                Monto = monto,
                SaldoDisponible = cuenta.Saldo,
                Fecha = DateTime.UtcNow.Date + DateTime.UtcNow.TimeOfDay
            };

            _appDbContext.Transacciones.Add(transaccion);
            _appDbContext.SaveChanges();

            return transaccion;
        }

        // Número de cuenta aleatorio
        private int GenerarNumeroCuenta()
        {
            var random = new Random();
            int numeroCuenta;

            do
            {
                numeroCuenta = random.Next(1000, 10000);
            }
            while (_appDbContext.CuentaBancarias.Any(c => c.NumeroCuenta == numeroCuenta));

            return numeroCuenta;
        }

    }
}
