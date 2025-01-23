namespace ChallengeAPISegurarse.Interfaces
{
    public interface IClienteValidationService
    {
        Task<bool> ValidarClienteAsync(string nombre, string apellido);
    }
}
