using System;
using System.Collections.Generic;

namespace Pierres.Models
{
  public class Flavor
  {
    public Flavor()
    {
      this.JoinEntities = new HashSet<Taste>();
    }
    public int FlavorId { get; set; }
    public string Name { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<Taste> JoinEntities { get; set; }
  }
}