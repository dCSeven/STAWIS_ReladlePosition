using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stawis {
  public partial class Form1 : Form {
    Model model;
    Thread refreshCyclic;
    Thread receiveMessage;
    int noOfStations;
  
    public Form1() {
      InitializeComponent();
      model = new Model(this);
      noOfStations = model.getStationCount();
      Panel[] panels = { pnlC1, pnlC2, pnlC3, pnlD1, pnlD2, pnlChgl };

      for (int st = 0; st < noOfStations; st++) {
        Station station = model.getStation(st);
        station.PanelName = panels[st];
        station.Center = calcStationCenter(panels[st]);
        station.Refresh = true;
        model.calcReladlingPoints(st);
      }
      
      // parkposition of ladle 1 (=charging ladle) is initialized with original charging ladle position from form
      Point pCenter = new Point();
      pCenter.X = pnlChgl.Left + pnlChgl.Width/2;
      pCenter.Y = pnlChgl.Top + pnlChgl.Height/2;
      model.setParkPositionOfLadle(1, pCenter); // ladleno and parkposition
    }

    private void Form1_Load(object sender, EventArgs e) {
      RefreshFormThread refreshThread = new RefreshFormThread(this, model);
      refreshCyclic = new Thread(new ThreadStart(refreshThread.RefreshForm));
      refreshCyclic.IsBackground = true;
      refreshCyclic.Start();

      ReceiveDataThread receiveThread = new ReceiveDataThread(model);
      receiveMessage = new Thread(new ThreadStart(receiveThread.ReceiveData));
      receiveMessage.IsBackground = true;
      receiveMessage.Start();

    }

    public void redrawAll() {
      moveStations();
      drawStations();
      showOrders();
      showAlarms();
    }

    void drawStations() {
      for (int stNo = noOfStations - 1; stNo >= 0; stNo--) {
        drawStation(stNo);
      }
    }

    void moveStations() {
      for (int stNo = 0; stNo < noOfStations; stNo++) {
        moveStation(stNo);
      }
    }

    void moveStation(int stNo) {
      Station st = model.getStation(stNo);
      if (st != null && st.Move) {
        Panel p = st.PanelName;
        p.Left = st.Center.X - p.Width / 2;
        p.Top = st.Center.Y - p.Height / 2;
        st.Move = false;
      }
    }
 
    void drawStation(int st) {
      Station station = model.getStation(st);
      if (station != null && station.Refresh) {
        Panel panel = station.PanelName;
        Graphics g = panel.CreateGraphics();
        if (station is Converter) {
          panel.BackgroundImage = States.convPic[station.State];
        } else if (station is Desulphurization) {
          panel.BackgroundImage = States.desPic[station.State];
        } else {
          panel.BackgroundImage = States.ladlePic[station.State];
        }

        panel.Refresh();

        string stationId = station.Type + station.Number;
        Order o = station.Order;
        string orderId = null;
        if (o != null) {
          orderId = o.OrderId;
        }
        Font formatId = new Font("Arial", 9, FontStyle.Bold);
        Font formatDur = new Font("Arial", 8, FontStyle.Bold);
     // zeichne stationId-String
        SizeF sF = g.MeasureString(stationId, formatId);
        float w = (panel.Width - sF.Width) / 2;   // Abstand von links
        float h = (panel.Height - sF.Height) / 5 * 3;  // Abstand von oben für StationId
        g.DrawString(stationId, formatId, Brushes.Black, w, h);
     // zeichne orderId-String
        sF = g.MeasureString(orderId, formatId);
        w = (panel.Width - sF.Width) / 2;   // Abstand von links
        h = (panel.Height - sF.Height) / 5 * 3 + 10;  // Abstand von oben für OrderId
        g.DrawString(orderId, formatId, Brushes.White, w, h);

        // zeichne Treatmentduration
        if (!(station is Ladle)) {
          string processDuration = "";
          if (station is Converter && (station.State == States.BUSY || station.State == States.TAPPING)) {
            processDuration = "Dur: " + station.CurrentProcessDuration.ToString() + " sec";
          } else if (station is Desulphurization && station.State == States.BUSY) {
              processDuration = "Dur: " + station.CurrentProcessDuration.ToString() + " sec";
          }
          sF = g.MeasureString(processDuration, formatDur);
          w = (panel.Width - sF.Width) / 2;
          h = (panel.Height - sF.Height) / 5 * 3 + 30;
          g.DrawString(processDuration, formatDur, Brushes.Red, w, h);
        }
        station.Refresh = false;
      }
    }

    public Point calcStationCenter(Panel panel) {
      Point center = new Point();
      center.X = panel.Location.X + panel.Width / 2;
      center.Y = panel.Location.Y + panel.Height / 2;
      return center;
    }

   void showOrders() {
      if (model.OrderRefresh) {
        List<Order> orderPool = model.OrderPool;
        lstOrders.Items.Clear();
        foreach (Order o in orderPool) {
          ListViewItem lvi = new ListViewItem(o.OrderId);
          lvi.SubItems.Add(o.Weight.ToString());
          lstOrders.Items.Add(lvi);
        }
        model.OrderRefresh = false;
      }
    }

    void showAlarms() {
      if (model.AlarmRefresh) {
        List<string> alarmList = model.AlarmList;
        while (alarmList.Count > 0) {
          cbxAlarm.Items.Add(alarmList[0]);
          alarmList.RemoveAt(0);
          cbxAlarm.SelectedIndex = cbxAlarm.Items.Count - 1;
        }
      }
    }

    private void cmdClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
      refreshCyclic.Abort();
    }

    private void cbxAlarm_KeyPress(object sender, KeyPressEventArgs e) {
      int index = cbxAlarm.SelectedIndex;
      if (index >= 0) {
        cbxAlarm.Items.RemoveAt(index);
      }
      if (cbxAlarm.Items.Count > 0) {
        cbxAlarm.SelectedIndex = cbxAlarm.Items.Count - 1;
      }
      if (cbxAlarm.Items.Count == 0) {
        cbxAlarm.Text = "";
      }
    }

    private void cmdReset_Click(object sender, EventArgs e) {
      model.resetAll();
    }
  }
}
