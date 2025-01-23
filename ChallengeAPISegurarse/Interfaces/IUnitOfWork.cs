using ChallengeAPISegurarse.Entities;

namespace ChallengeAPISegurarse.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Cliente> Clientes { get; }
        IGenericRepository<Poliza> Polizas { get; }
        Task<int> CompleteAsync();
    }
}
