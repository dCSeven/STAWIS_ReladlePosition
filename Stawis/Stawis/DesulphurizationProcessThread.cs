using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace Stawis {
  public class DesulphurizationProcessThread {
    private Station station;

    public DesulphurizationProcessThread(Station station) {
      this.station = station;
    }

    public void SimulateDesulphurizationProcess() { // Thread für Simulieren des Entschwefelungsprozesses
      station.CurrentProcessDuration = 0;
      while ((Thread.CurrentThread.ThreadState & ThreadState.Running) == ThreadState.Running) {
        Thread.Sleep(States.PROCESSSTEP * 1000);
        station.CurrentProcessDuration += States.PROCESSSTEP;
        station.Refresh = true;
      }
    }
  }
}
