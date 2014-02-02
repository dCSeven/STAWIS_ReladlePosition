using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stawis {
  public class RefreshFormThread {
    Form1 form;
    Model model;
    delegate void RefreshFormDelegate();

    public RefreshFormThread(Form1 form, Model model) {
      this.form = form;
      this.model = model;
    }

    public void RefreshForm() {
      RefreshFormDelegate refresh = new RefreshFormDelegate(form.redrawAll);
      while ((Thread.CurrentThread.ThreadState & ThreadState.Running)
            == ThreadState.Running) {
          try {
            form.Invoke(refresh);
          } catch (Exception e) {
            String x = e.Message; // damit keine Warnung kommt
          }
          Thread.Sleep(100);
      }
    }
  }
}
