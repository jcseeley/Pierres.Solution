namespace Pierres.Models
{
  public class Taste
    {       
        public int TasteId { get; set; }
        public int TreatId { get; set; }
        public int FlavorId { get; set; }
        public virtual Treat Treat { get; set; }
        public virtual Flavor Flavor { get; set; }
    }
}