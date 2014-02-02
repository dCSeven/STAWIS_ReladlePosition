using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stawis {
  public static class States {
    public const int FREE = 0;
    public const int BUSY = 1;
    public const int CHARGING = 2;
    public const int TAPPING = 3;
    public const int READY = 4;
    public const int FINISHED = 5;
    public const int INACTIVE = 6;
    public const int ON = 1;
    public const int OFF = 0;
    public const int PROCESSSTEP = 1;

    public static Bitmap[] ladlePic = {
      Properties.Resources.LadleEmpty,  // 0
      Properties.Resources.LadleFull, // 1
      Properties.Resources.LadleTilt, //2
      Properties.Resources.LadleEmpty,  // 3
      Properties.Resources.LadleEmpty,  // 4
      Properties.Resources.LadleEmpty,  // 5
      Properties.Resources.LadleEmpty   // 6
    };

    public static Bitmap[] convPic = {
      Properties.Resources.ConvEmpty,  // 0
      Properties.Resources.ConvProcess, // 1
      Properties.Resources.ConvCharging, //2
      Properties.Resources.ConvTilt,  // 3
      Properties.Resources.ConvFull,  // 4
      Properties.Resources.ConvFinished,  // 5
      Properties.Resources.ConvDeactiv   // 6
    };

    public static Bitmap[] desPic = {
      Properties.Resources.DesEmpty,  // 0
      Properties.Resources.DesProcess, // 1
      Properties.Resources.DesEmpty, //2
      Properties.Resources.DesEmpty,  // 3
      Properties.Resources.DesFull,  // 4
      Properties.Resources.DesFinished,  // 5
      Properties.Resources.DesEmpty   // 6
    };
  }
}
