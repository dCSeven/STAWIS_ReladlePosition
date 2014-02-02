using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace Stawis {
  public class TappingProcessThread {
    private Station station;

    public TappingProcessThread(Station station) {
      this.station = station;
    }

    public void SimulateTappingProcess() { // Thread für Simulieren des Converterprozesses
      station.CurrentProcessDuration = 0;
      while ((Thread.CurrentThread.ThreadState & ThreadState.Running) == ThreadState.Running) {
        Thread.Sleep(States.PROCESSSTEP * 1000);
        station.CurrentProcessDuration += States.PROCESSSTEP;
        station.Refresh = true;
      }
    }
  }
}
