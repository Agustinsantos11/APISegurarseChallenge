namespace ChallengeAPISegurarse.Entities
{
    public class Poliza
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Auto { get; set; }
        public decimal Costo { get; set; }
        public DateTime FechaVigencia { get; set; }

        public virtual Cliente? Cliente { get; set; }
    }
}
