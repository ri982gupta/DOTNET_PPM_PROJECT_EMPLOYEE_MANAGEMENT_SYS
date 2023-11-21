using System;
using System.Collections.Generic;
using System.Linq;

namespace PPM.Model
{
  [Serializable]
  public class Role
  {
    public int Id { get; set; }
    public string ?Name { get; set; }
  }
}