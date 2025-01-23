namespace ChallengeAPISegurarse.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DNI { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public virtual ICollection<Poliza>? Polizas { get; set; }
    }
}
