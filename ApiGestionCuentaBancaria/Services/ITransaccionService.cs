using ApiGestionBancaria.Data;
using ApiGestionBancaria.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System;

namespace ApiGestionBancaria.Services
{
    public interface ITransaccionService
    {
        List<Transaccion> GetTransaccionesByCuenta(int numeroCuenta);
  
    }

    public class TransaccionService : ITransaccionService
    {
        private readonly AppDbContext _appDbContext;

        public TransaccionService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //Filtrar registro de transacciones por # de cuenta
        public List<Transaccion> GetTransaccionesByCuenta(int numeroCuenta)
        {
            return _appDbContext.Transacciones.Where(c => c.NumeroCuenta == numeroCuenta).ToList();

        }

    }
}
