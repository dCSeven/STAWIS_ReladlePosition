using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Stawis {
  public class ConverterProcessThread {
    private Station station;

    public ConverterProcessThread(Station station) {
      this.station = station;
    }

    public void SimulateConverterProcess() { // Thread für Simulieren des Converterprozesses
      station.CurrentProcessDuration = 0;
      while ((Thread.CurrentThread.ThreadState & ThreadState.Running) == ThreadState.Running) {
        Thread.Sleep(States.PROCESSSTEP * 1000);
        station.CurrentProcessDuration += States.PROCESSSTEP;
        station.Refresh = true;
      }
    }
  }
}
