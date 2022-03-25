using System.Collections.Generic;

namespace Pierres.Models
{
  public class Treat
  {
    public Treat()
    {
      this.JoinEntities = new HashSet<Taste>();
    }
    public int TreatId { get; set; }
    public string Name { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<Taste> JoinEntities { get; set; }
  }
}