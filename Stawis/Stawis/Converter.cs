using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stawis {
  public class Converter : Station {
    public Converter(int number, int relX, int relY)
      : base("C", number, relX, relY) {
    }

  }
}
