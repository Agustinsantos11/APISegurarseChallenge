namespace ChallengeAPISegurarse.DTOs
{
    public class PolizaRequestDTO
    {
        public int ClienteId { get; set; }
        public string Auto { get; set; }
        public decimal Costo { get; set; }
        public DateTime FechaVigencia { get; set; }
    }

    
}
