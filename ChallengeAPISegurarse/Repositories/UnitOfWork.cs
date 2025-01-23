using ChallengeAPISegurarse.Context;
using ChallengeAPISegurarse.Entities;
using ChallengeAPISegurarse.Interfaces;

namespace ChallengeAPISegurarse.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGenericRepository<Cliente> Clientes { get; private set; }
        public IGenericRepository<Poliza> Polizas { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Clientes = new GenericRepository<Cliente>(_context);
            Polizas = new GenericRepository<Poliza>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
